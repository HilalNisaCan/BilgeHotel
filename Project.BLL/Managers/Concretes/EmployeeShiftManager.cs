using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Project.BLL.DtoClasses;
using Project.BLL.Managers.Abstracts;
using Project.Dal.ContextClasses;
using Project.Dal.Repositories.Abstracts;
using Project.Dal.Repositories.Concretes;
using Project.Entities.Enums;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Managers.Concretes
{ /// <summary>
  /// Çalışanların vardiyalarını yöneten manager sınıfıdır (ShiftStart, ShiftEnd yapısına göre uyarlandı).
  /// </summary>
    public class EmployeeShiftManager : BaseManager<EmployeeShiftDto, EmployeeShift>, IEmployeeShiftManager
    {
        private readonly IEmployeeShiftRepository _shiftRepository;
        private readonly IEmployeeShiftAssignmentRepository _assignmentRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public EmployeeShiftManager(
            IEmployeeShiftRepository shiftRepository,
            IEmployeeShiftAssignmentRepository assignmentRepository,
            IEmployeeRepository employeeRepository,
            IMapper mapper)
            : base(shiftRepository, mapper)
        {
            _shiftRepository = shiftRepository;
           _assignmentRepository = assignmentRepository;
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Çalışana yeni vardiya oluşturur ve atar.
        /// </summary>
        public async Task<bool> AssignShiftAsync(int employeeId, DateTime startDate, DateTime endDate)
        {
            Employee? employee = await _employeeRepository.GetByIdAsync(employeeId);
            if (employee == null) return false;

            // Yeni vardiya oluştur
            EmployeeShift shift = new EmployeeShift
            {
                ShiftDate = startDate.Date,
                ShiftStart = startDate.TimeOfDay,
                ShiftEnd = endDate.TimeOfDay,
                ShiftType = ShiftType.Morning,
                IsDayOff = false,
                Description = "Otomatik atanan vardiya"
            };

            await _shiftRepository.AddAsync(shift);

            // Vardiyayı çalışana ata
            EmployeeShiftAssignment assignment = new EmployeeShiftAssignment
            {
                EmployeeId = employeeId,
                EmployeeShiftId = shift.Id,
                AssignedDate = DateTime.UtcNow,
                ShiftStatus = ShiftStatus.Assigned
            };

            await _assignmentRepository.AddAsync(assignment);
            return true;
        }



        /// <summary>
        /// Yeni vardiya oluşturur ve ID’yi döner.
        /// </summary>
        public async Task<int> CreateAndReturnIdAsync(EmployeeShiftDto dto)
        {
            EmployeeShift entity = _mapper.Map<EmployeeShift>(dto);
            await _shiftRepository.AddAsync(entity);
            return entity.Id;
        }

        /// <summary>
        /// Tüm vardiyaları ve atanan çalışanları getirir.
        /// </summary>
        public async Task<List<EmployeeShiftDto>> GetAllWithAssignmentsAsync()
        {
            List<EmployeeShift> shifts = (await _shiftRepository.GetAllWithIncludeAsync(
                x => true,
                q => q.Include(s => s.ShiftAssignments).ThenInclude(a => a.Employee)
            )).ToList();

            // Loglama sadece test içindir, sunumda kaldırılabilir
            foreach (EmployeeShift shift in shifts)
            {
                Console.WriteLine($"[TEST] Vardiya ID: {shift.Id}, Tarih: {shift.ShiftDate:yyyy-MM-dd}, Atama Sayısı: {shift.ShiftAssignments.Count}");

                foreach (EmployeeShiftAssignment assignment in shift.ShiftAssignments)
                {
                    string employeeName = assignment.Employee != null
                        ? $"{assignment.Employee.FirstName} {assignment.Employee.LastName}"
                        : "(Employee NULL)";
                    Console.WriteLine($" --> Atanan: {employeeName}, Tarih: {assignment.AssignedDate:yyyy-MM-dd}");
                }
            }

            List<EmployeeShiftDto> shiftDtos = _mapper.Map<List<EmployeeShiftDto>>(shifts);
            return shiftDtos;
        }

        /// <summary>
        /// Belirtilen çalışanın belirtilen tarihler arasındaki fazla mesai süresini hesaplar.
        /// </summary>
        public async Task<double> CalculateOvertimeAsync(int employeeId, DateTime startDate, DateTime endDate)
        {
            List<EmployeeShiftAssignment> shifts = (await _assignmentRepository
                .GetAllWithIncludeAsync(
                    predicate: x => x.EmployeeId == employeeId &&
                                    x.EmployeeShift.ShiftDate >= startDate &&
                                    x.EmployeeShift.ShiftDate <= endDate,
                    include: x => x.Include(y => y.EmployeeShift)
                )).ToList();

            double overtimeHours = 0;

            foreach (EmployeeShiftAssignment shift in shifts)
            {
                TimeSpan duration;

                if (shift.EmployeeShift.ShiftEnd > shift.EmployeeShift.ShiftStart)
                    duration = shift.EmployeeShift.ShiftEnd - shift.EmployeeShift.ShiftStart;
                else
                    duration = (TimeSpan.FromHours(24) - shift.EmployeeShift.ShiftStart) + shift.EmployeeShift.ShiftEnd;

                double worked = duration.TotalHours;

                if (worked > 8)
                    overtimeHours += worked - 8;
            }

            return overtimeHours;
        }

        /// <summary>
        /// Belirtilen ID’ye sahip vardiyayı ve ilişkili tüm atamaları siler.
        /// </summary>
        public async Task<bool> DeleteShiftByIdAsync(int shiftId)
        {
            EmployeeShift shift = await _shiftRepository.GetByIdAsync(shiftId);
            if (shift == null)
                return false;

            List<EmployeeShiftAssignment> assignments = _assignmentRepository
                .Where(x => x.EmployeeShiftId == shiftId)
                .ToList();

            foreach (EmployeeShiftAssignment item in assignments)
            {
                await _assignmentRepository.RemoveAsync(item);
            }

            await _shiftRepository.RemoveAsync(shift);
            return true;
        }
        /// <summary>
        /// Çalışanın belirtilen tarih aralığında toplam maaşını ve çalışma süresini hesaplar.
        /// </summary>
        public async Task<(decimal Salary, double TotalWorkedHours)> CalculateSalaryAsync(int employeeId, DateTime startDate, DateTime endDate)
        {
            Employee? employee = await _employeeRepository.GetByIdAsync(employeeId);
            if (employee == null)
                return (0, 0);

            // Aylık maaşlı personelse saat hesabı yapılmaz
            if (employee.SalaryType == SalaryType.Monthly)
                return (employee.MonthlySalary, 0);

            // Saatlik çalışanlar için vardiyaları al
            List<EmployeeShiftAssignment> shifts = (await _assignmentRepository
                .GetAllWithIncludeAsync(
                    predicate: x => x.EmployeeId == employeeId &&
                                    x.EmployeeShift.ShiftDate >= startDate &&
                                    x.EmployeeShift.ShiftDate <= endDate,
                    include: x => x.Include(y => y.EmployeeShift)
                )).ToList();

            double totalWorkedHours = 0;

            foreach (EmployeeShiftAssignment shift in shifts)
            {
                TimeSpan duration;

                // Gece vardiyası düzeltmesi
                if (shift.EmployeeShift.ShiftEnd > shift.EmployeeShift.ShiftStart)
                    duration = shift.EmployeeShift.ShiftEnd - shift.EmployeeShift.ShiftStart;
                else
                    duration = (TimeSpan.FromHours(24) - shift.EmployeeShift.ShiftStart) + shift.EmployeeShift.ShiftEnd;

                totalWorkedHours += duration.TotalHours;
            }

            decimal totalSalary = (decimal)totalWorkedHours * employee.HourlyWage;
            return (Math.Round(totalSalary, 2), totalWorkedHours);
        }

        /// <summary>
        /// Vardiya bilgilerini günceller. Atamaları korur.
        /// </summary>
        public async Task<bool> UpdateShiftAsync(EmployeeShiftDto dto)
        {
            EmployeeShift? original = await _shiftRepository.GetByIdAsync(dto.Id);
            if (original == null)
                return false;

            // Atama bilgilerini kaybetmemek için sakla
            List<EmployeeShiftAssignment> currentAssignments = original.ShiftAssignments.ToList();

            // Mapping işlemi
            _mapper.Map(dto, original);
            original.ShiftAssignments = currentAssignments;

            original.ModifiedDate = DateTime.Now;
            original.Status = DataStatus.Updated;

            await _shiftRepository.UpdateAsync(original);
            return true;
        }
    }    
}
