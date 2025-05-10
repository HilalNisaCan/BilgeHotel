using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Project.BLL.DtoClasses;
using Project.BLL.Managers.Abstracts;
using Project.Dal.Repositories.Abstracts;
using Project.Entities.Enums;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Managers.Concretes
{

  //  IRepository<Employee> repositoryForInclude
    public class EmployeeManager : BaseManager<EmployeeDto, Employee>, IEmployeeManager
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IEmployeeShiftAssignmentRepository _shiftAssignmentRepository;
        private readonly IEmployeeShiftRepository _shiftRepository;
        private readonly IMapper _mapper;

        public EmployeeManager(IEmployeeRepository employeeRepository,
                               IEmployeeShiftAssignmentRepository shiftAssignmentRepository,
                               IEmployeeShiftRepository shiftRepository,
                               IMapper mapper)
            : base(employeeRepository, mapper)
        {
            _employeeRepository = employeeRepository;
            _shiftAssignmentRepository = shiftAssignmentRepository;
            _shiftRepository = shiftRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Yeni çalışan oluşturur.
        /// </summary>
        public async Task<int> AddEmployeeAsync(EmployeeDto dto)
        {
            Employee employee = _mapper.Map<Employee>(dto);
            await _employeeRepository.AddAsync(employee);
            return employee.Id;
        }

        /// <summary>
        /// ID’ye göre çalışanı getirir.
        /// </summary>
        public async Task<EmployeeDto> GetEmployeeByIdAsync(int id)
        {
            Employee? employee = await _employeeRepository.GetByIdAsync(id);
            return _mapper.Map<EmployeeDto>(employee);
        }

        /// <summary>
        /// Tüm çalışanları getirir.
        /// </summary>
        public async Task<List<EmployeeDto>> GetAllEmployeesAsync()
        {
            List<Employee> employees = (await _employeeRepository.GetAllAsync()).ToList();
            return _mapper.Map<List<EmployeeDto>>(employees);
        }

        /// <summary>
        /// Çalışan bilgilerini günceller.
        /// </summary>
        public async Task<bool> UpdateEmployeeAsync(EmployeeDto dto)
        {
            Employee? employee = await _employeeRepository.GetByIdAsync(dto.Id);
            if (employee == null) return false;

            _mapper.Map(dto, employee);
            await _employeeRepository.UpdateAsync(employee);
            return true;
        }

        /// <summary>
        /// Çalışanı siler.
        /// </summary>
        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            Employee? employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null) return false;

            await _employeeRepository.RemoveAsync(employee);
            return true;
        }

        /// <summary>
        /// Çalışana belirtilen vardiyayı atar.
        /// </summary>
        public async Task<bool> AssignShiftAsync(int employeeId, int shiftId, DateTime assignedDate)
        {
            EmployeeShiftAssignment assignment = new EmployeeShiftAssignment
            {
                EmployeeId = employeeId,
                EmployeeShiftId = shiftId,
                AssignedDate = assignedDate
            };

            await _shiftAssignmentRepository.AddAsync(assignment);
            return true;
        }
        /// <summary>
        /// Vardiyalardan toplam saatleri toplayarak maaş hesaplar.
        /// </summary>
        public async Task<decimal> CalculateSalaryAsync(int employeeId, decimal hourlyRate)
        {
            List<EmployeeShiftAssignment> assignments = (await _shiftAssignmentRepository.GetAllAsync(a => a.EmployeeId == employeeId)).ToList();
            decimal totalHours = 0;

            foreach (EmployeeShiftAssignment assignment in assignments)
            {
                EmployeeShift? shift = await _shiftRepository.GetByIdAsync(assignment.EmployeeShiftId);
                if (shift != null)
                {
                    totalHours += (decimal)(shift.ShiftEnd - shift.ShiftStart).TotalHours;
                }
            }

            return totalHours * hourlyRate;
        }

        /// <summary>
        /// Çalışanları kullanıcı ve profil bilgileriyle birlikte getirir.
        /// </summary>
        public async Task<List<EmployeeDto>> GetAllEmployeesWithDetailsAsync()
        {
            List<Employee> employees =( await _employeeRepository.GetAllWithIncludeAsync(
                null,
                q => q.Include(e => e.User)
                      .ThenInclude(u => u.UserProfile)
            )).ToList();

            return _mapper.Map<List<EmployeeDto>>(employees);
        }

        /// <summary>
        /// Belirli pozisyondaki çalışanları getirir.
        /// </summary>
        public async Task<List<EmployeeDto>> GetByPositionAsync(EmployeePosition position)
        {
            List<Employee> employees = (await _employeeRepository.GetAllAsync(e => e.Position == position)).ToList();
            return _mapper.Map<List<EmployeeDto>>(employees);
        }

        /// <summary>
        /// Birden fazla pozisyondaki aktif çalışanları getirir.
        /// </summary>
        public async Task<List<EmployeeDto>> GetByPositionsAsync(EmployeePosition[] positions)
        {
            List<Employee> employees = (await _employeeRepository
                .GetAllAsync(e => positions.Contains(e.Position) && e.IsActive)).ToList();

            return _mapper.Map<List<EmployeeDto>>(employees);
        }
    }
}
