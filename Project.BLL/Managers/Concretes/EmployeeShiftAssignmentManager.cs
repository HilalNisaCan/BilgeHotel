using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Project.BLL.DtoClasses;
using Project.BLL.Managers.Abstracts;
using Project.Dal.Repositories.Abstracts;
using Project.Dal.Repositories.Concretes;
using Project.Entities.Enums;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Managers.Concretes
{
    public class EmployeeShiftAssignmentManager : BaseManager<EmployeeShiftAssignmentDto, EmployeeShiftAssignment>, IEmployeeShiftAssignmentManager
    {
        private readonly IEmployeeShiftAssignmentRepository _assignmentRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IEmployeeShiftRepository _employeeShiftRepository;
        private readonly IMapper _mapper;

        public EmployeeShiftAssignmentManager(IEmployeeShiftAssignmentRepository assignmentRepository,
                                              IEmployeeRepository employeeRepository,
                                              IEmployeeShiftRepository employeeShiftRepository,
                                              IMapper mapper)
            : base(assignmentRepository, mapper)
        {
            _assignmentRepository = assignmentRepository;
            _employeeRepository = employeeRepository;
            _employeeShiftRepository = employeeShiftRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Belirli bir çalışana belirtilen vardiyayı atar. Aynı güne aynı çalışana tekrar atama yapılmaz.
        /// </summary>
        public async Task<bool> AssignShiftAsync(int employeeId, int shiftId, DateTime date)
        {
            var employee = await _employeeRepository.GetByIdAsync(employeeId);
            var shift = await _employeeShiftRepository.GetByIdAsync(shiftId);

            if (employee == null || shift == null)
                throw new Exception("Çalışan veya vardiya bulunamadı.");

            // ✅ Aynı çalışana aynı tarihte vardiya ataması yapılamaz
            var existingAssignment = await _assignmentRepository.FirstOrDefaultAsync(a =>
                a.EmployeeId == employeeId &&
                a.EmployeeShift.ShiftDate.Date == date.Date);

            if (existingAssignment != null)
                throw new Exception("Bu çalışana bu tarihte zaten bir vardiya atanmış.");

            var assignment = new EmployeeShiftAssignment
            {
                EmployeeId = employeeId,
                EmployeeShiftId = shiftId,
                AssignedDate = DateTime.UtcNow
            };

            await _assignmentRepository.AddAsync(assignment);
            return true;
        }

        public async Task<List<EmployeeShiftAssignmentDto>> GetAllWithDetailsAsync()
        {
            List<EmployeeShiftAssignment> entities = await _assignmentRepository.GetAllWithIncludeAsync();
            return _mapper.Map<List<EmployeeShiftAssignmentDto>>(entities);
        }

      
        public async Task<List<EmployeeShiftAssignmentDto>> GetAssignmentsForWeekAsync(int employeeId, DateTime weekStartDate)
        {
            DateTime start = weekStartDate.Date;
            DateTime end = start.AddDays(7);

            // 🔁 Veriyi repository'den alıyoruz (düzenlenmiş haliyle)
            List<EmployeeShiftAssignment> allAssignments = await _assignmentRepository.GetAllWithEmployeeAndShiftAsync();

            // 🔍 Şartlara göre filtreliyoruz
            List<EmployeeShiftAssignment> filtered = allAssignments
                .Where(x => x.EmployeeId == employeeId &&
                            x.AssignedDate.Date >= start &&
                            x.AssignedDate.Date < end)
                .ToList();

            // 🎯 DTO'ya mapleyip dönüyoruz
            return _mapper.Map<List<EmployeeShiftAssignmentDto>>(filtered);
        }


        public async Task<List<EmployeeShiftAssignmentDto>> GetEmployeeShiftsAsync(int employeeId)
        {
            var assignments = await _assignmentRepository.GetAllAsync(a => a.EmployeeId == employeeId);
            return _mapper.Map<List<EmployeeShiftAssignmentDto>>(assignments);
        }

        public async Task<bool> RemoveShiftAssignmentAsync(int assignmentId)
        {
            var assignment = await _assignmentRepository.GetByIdAsync(assignmentId);
            if (assignment == null)
                return false;

            await _assignmentRepository.RemoveAsync(assignment);
            return true;
        }


        public async Task<bool> ValidateShiftAssignmentAsync(int employeeId, int shiftId)
        {
            var existingAssignment = await _assignmentRepository.GetAsync(a => a.EmployeeId == employeeId && a.EmployeeShiftId == shiftId);
            return existingAssignment == null; // ✅ Eğer kayıt yoksa vardiya atanabilir
        }

        /// <summary>
        /// Aynı güne aynı çalışana tekrar vardiya ataması yapılmasını engeller.
        /// </summary>
        public async Task<bool> ValidateShiftAssignmentAsync(int employeeId, DateTime date)
        {
            // Aynı tarihte zaten atama yapılmış mı kontrol et
            var existingAssignment = await _assignmentRepository.FirstOrDefaultAsync(a =>
                a.EmployeeId == employeeId &&
                a.EmployeeShift.ShiftDate.Date == date.Date);

            // Eğer varsa çakışma vardır → false döner
            return existingAssignment == null;
        }

    }
}