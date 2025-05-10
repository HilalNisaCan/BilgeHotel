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
        /// Belirli bir çalışana belirtilen vardiyayı atar. Aynı güne tekrar atama yapılmaz.
        /// </summary>
        public async Task<bool> AssignShiftAsync(int employeeId, int shiftId, DateTime date)
        {
            Employee? employee = await _employeeRepository.GetByIdAsync(employeeId);
            EmployeeShift? shift = await _employeeShiftRepository.GetByIdAsync(shiftId);

            if (employee == null || shift == null)
                throw new Exception("Çalışan veya vardiya bulunamadı.");

            // Aynı tarihe daha önce atama yapılmış mı kontrol et
            EmployeeShiftAssignment? existingAssignment = await _assignmentRepository.FirstOrDefaultAsync(a =>
                a.EmployeeId == employeeId &&
                a.EmployeeShift.ShiftDate.Date == date.Date);

            if (existingAssignment != null)
                throw new Exception("Bu çalışana bu tarihte zaten bir vardiya atanmış.");

            // Yeni atama yapılır
            EmployeeShiftAssignment assignment = new EmployeeShiftAssignment
            {
                EmployeeId = employeeId,
                EmployeeShiftId = shiftId,
                AssignedDate = DateTime.UtcNow
            };

            await _assignmentRepository.AddAsync(assignment);
            return true;
        }



        /// <summary>
        /// Belirli çalışanın bir haftalık vardiya atamalarını getirir.
        /// </summary>
        public async Task<List<EmployeeShiftAssignmentDto>> GetAssignmentsForWeekAsync(int employeeId, DateTime weekStartDate)
        {
            DateTime start = weekStartDate.Date;
            DateTime end = start.AddDays(7);

            List<EmployeeShiftAssignment> allAssignments = await _assignmentRepository.GetAllWithEmployeeAndShiftAsync();

            List<EmployeeShiftAssignment> filtered = allAssignments
                .Where(x => x.EmployeeId == employeeId &&
                            x.AssignedDate.Date >= start &&
                            x.AssignedDate.Date < end)
                .ToList();

            return _mapper.Map<List<EmployeeShiftAssignmentDto>>(filtered);
        }







    }
}