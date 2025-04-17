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
            var employee = await _employeeRepository.GetByIdAsync(employeeId);
            if (employee == null) return false;

            var shift = new EmployeeShift
            {
                ShiftDate = startDate.Date,
                ShiftStart = startDate.TimeOfDay,
                ShiftEnd = endDate.TimeOfDay,
                ShiftType = ShiftType.Morning, // Enum'dan mevcut olan kullanıldı
                IsDayOff = false,
                Description = "Otomatik atanan vardiya"
            };

            await _shiftRepository.AddAsync(shift);

            var assignment = new EmployeeShiftAssignment
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
        /// Haftalık toplam saat hesaplama.
        /// </summary>
        public async Task<double> CalculateWeeklyHoursAsync(int employeeId, DateTime weekStartDate)
        {
            var weekEnd = weekStartDate.AddDays(7);
            var assignments = await _assignmentRepository.GetAllAsync(x =>
                x.EmployeeId == employeeId &&
                x.EmployeeShift.ShiftDate >= weekStartDate &&
                x.EmployeeShift.ShiftDate <= weekEnd);

            return assignments.Sum(a => (a.EmployeeShift.ShiftEnd - a.EmployeeShift.ShiftStart).TotalHours);
        }

        ///// <summary>
        ///// Fazla mesai hesaplama.
        ///// </summary>
        //public async Task<double> CalculateOvertimeAsync(int employeeId, DateTime weekStartDate)
        //{
        //    var totalHours = await CalculateWeeklyHoursAsync(employeeId, weekStartDate);
        //    return totalHours > 40 ? totalHours - 40 : 0;
        //}

        ///// <summary>
        ///// Aylık maaş hesaplama.
        ///// </summary>
        //public async Task<decimal> CalculateSalaryAsync(int employeeId, int month, int year)
        //{
        //    var employee = await _employeeRepository.GetByIdAsync(employeeId);
        //    if (employee == null) return 0;

        //    var assignments = await _assignmentRepository.GetAllAsync(x =>
        //        x.EmployeeId == employeeId &&
        //        x.EmployeeShift.ShiftDate.Month == month &&
        //        x.EmployeeShift.ShiftDate.Year == year);

        //    var totalHours = assignments.Sum(a => (a.EmployeeShift.ShiftEnd - a.EmployeeShift.ShiftStart).TotalHours);
        //    var overtimeHours = totalHours > 160 ? totalHours - 160 : 0;
        //    var baseSalary = (decimal)totalHours * employee.HourlyWage;
        //    var overtimePay = (decimal)overtimeHours * employee.HourlyWage * 1.5m;

        //    return baseSalary + overtimePay;
        //}

        /// <summary>
        /// Çalışan için izin günü tanımlar.
        /// </summary>
        public async Task<bool> SetDayOffAsync(int employeeId, DateTime day)
        {
            var shift = new EmployeeShift
            {
                ShiftDate = day.Date,
                ShiftStart = new TimeSpan(0, 0, 0),
                ShiftEnd = new TimeSpan(0, 0, 0),
                ShiftType = ShiftType.DayOff,
                IsDayOff = true,
                Description = "İzin günü"
            };

            await _shiftRepository.AddAsync(shift);

            var assignment = new EmployeeShiftAssignment
            {
                EmployeeId = employeeId,
                EmployeeShiftId = shift.Id,
                AssignedDate = DateTime.UtcNow,
                ShiftStatus = ShiftStatus.Assigned
            };

            await _assignmentRepository.AddAsync(assignment);
            return true;
        }

        public async Task<int> CreateAndReturnIdAsync(EmployeeShiftDto dto)
        {
            EmployeeShift entity = _mapper.Map<EmployeeShift>(dto);
            await _shiftRepository.AddAsync(entity);
            return entity.Id;
        }

        public async Task<List<EmployeeShiftDto>> GetAllWithAssignmentsAsync()
        {
            List<EmployeeShift> shifts = (await _shiftRepository.GetAllWithIncludeAsync(
            x => true,
            q => q.Include(s => s.ShiftAssignments).ThenInclude(a => a.Employee)
            )).ToList();
            // 🔍 Logla her vardiyayı ve çalışanları
            foreach (var shift in shifts)
            {
                Console.WriteLine($"[TEST] Vardiya ID: {shift.Id}, Tarih: {shift.ShiftDate:yyyy-MM-dd}, Atama Sayısı: {shift.ShiftAssignments.Count}");

                foreach (var assignment in shift.ShiftAssignments)
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
            foreach (var shift in shifts)
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

       




        public async Task<bool> DeleteShiftByIdAsync(int shiftId)
        {
            // önce vardiyayı bul
            EmployeeShift shift = await _shiftRepository.GetByIdAsync(shiftId);
            if (shift == null)
                return false;

            // ona bağlı atamaları bul
            List<EmployeeShiftAssignment> assignments = _assignmentRepository
           .Where(x => x.EmployeeShiftId == shiftId)
          .ToList();

            // onları sil
            foreach (var item in assignments)
            {
                await _assignmentRepository.RemoveAsync(item);
            }

            // sonra vardiyayı sil
            await _shiftRepository.RemoveAsync(shift);

            return true;
        }

        public async Task<(decimal Salary, double TotalWorkedHours)> CalculateSalaryAsync(int employeeId, DateTime startDate, DateTime endDate)
        {
            Employee? employee = await _employeeRepository.GetByIdAsync(employeeId);
            if (employee == null)
                return (0, 0);

            if (employee.SalaryType == SalaryType.Monthly)
                return (employee.MonthlySalary, 0); // Aylık maaşta çalışılan saat önemli değil

            List<EmployeeShiftAssignment> shifts = (await _assignmentRepository
                .GetAllWithIncludeAsync(
                    predicate: x => x.EmployeeId == employeeId &&
                                    x.EmployeeShift.ShiftDate >= startDate &&
                                    x.EmployeeShift.ShiftDate <= endDate,
                    include: x => x.Include(y => y.EmployeeShift)
                )).ToList();

            double totalWorkedHours = 0;

            foreach (var shift in shifts)
            {
                TimeSpan duration;

                // 🔁 Gece vardiyası düzeltmesi
                if (shift.EmployeeShift.ShiftEnd > shift.EmployeeShift.ShiftStart)
                    duration = shift.EmployeeShift.ShiftEnd - shift.EmployeeShift.ShiftStart;
                else
                    duration = (TimeSpan.FromHours(24) - shift.EmployeeShift.ShiftStart) + shift.EmployeeShift.ShiftEnd;

                totalWorkedHours += duration.TotalHours;
            }

            decimal totalSalary = (decimal)totalWorkedHours * employee.HourlyWage;
            return (Math.Round(totalSalary, 2), totalWorkedHours);
        }
    }    
}
