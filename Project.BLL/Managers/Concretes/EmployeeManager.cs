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
    //    private readonly IRepository<Employee> _repositoryForInclude;
        public EmployeeManager(IEmployeeRepository employeeRepository,
                               IEmployeeShiftAssignmentRepository shiftAssignmentRepository,
                               IEmployeeShiftRepository shiftRepository,
                               IMapper mapper)
            : base(employeeRepository, mapper)
        {
            _employeeRepository = employeeRepository;
      //      _repositoryForInclude = repositoryForInclude; // assign ettik
            _shiftAssignmentRepository = shiftAssignmentRepository;
            _shiftRepository = shiftRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Yeni çalışan oluşturur.
        /// </summary>
        public async Task<int> AddEmployeeAsync(EmployeeDto dto)
        {
            var employee = _mapper.Map<Employee>(dto);
            await _employeeRepository.AddAsync(employee);
            return employee.Id;
        }

        /// <summary>
        /// ID'ye göre çalışan getirir.
        /// </summary>
        public async Task<EmployeeDto> GetEmployeeByIdAsync(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            return _mapper.Map<EmployeeDto>(employee);
        }

        /// <summary>
        /// Tüm çalışanları DTO listesi olarak getirir.
        /// </summary>
        public async Task<List<EmployeeDto>> GetAllEmployeesAsync()
        {
            var employees = await _employeeRepository.GetAllAsync();
            return _mapper.Map<List<EmployeeDto>>(employees);
        }

        /// <summary>
        /// Çalışan bilgilerini günceller.
        /// </summary>
        public async Task<bool> UpdateEmployeeAsync(EmployeeDto dto)
        {
            var employee = await _employeeRepository.GetByIdAsync(dto.Id);
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
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null) return false;

            await _employeeRepository.RemoveAsync(employee);
            return true;
        }

        /// <summary>
        /// Sabit maaş hesaplar: saatlik ücret x 160 saat.
        /// </summary>
        public async Task<decimal> GetManagerSalaryAsync(int employeeId, decimal hourlyRate)
        {
            return hourlyRate * 160;
        }

        /// <summary>
        /// Çalışana vardiya atar.
        /// </summary>
        public async Task<bool> AssignShiftAsync(int employeeId, int shiftId, DateTime assignedDate)
        {
            var assignment = new EmployeeShiftAssignment
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
            var assignments = await _shiftAssignmentRepository.GetAllAsync(a => a.EmployeeId == employeeId);
            decimal totalHours = 0;

            foreach (var assignment in assignments)
            {
                var shift = await _shiftRepository.GetByIdAsync(assignment.EmployeeShiftId);
                if (shift != null)
                {
                    totalHours += (decimal)(shift.ShiftEnd - shift.ShiftStart).TotalHours;
                }
            }

            return totalHours * hourlyRate;
        }

        /// <summary>
        /// Çalışanın toplam fazla mesai süresini getirir.
        /// </summary>
        public async Task<int> TrackOvertimeAsync(int employeeId)
        {
            var employee = await _employeeRepository.GetByIdAsync(employeeId);
            if (employee == null) return 0;

            int regularMonthlyHours = 160;
            return employee.TotalWorkedHours > regularMonthlyHours
                ? employee.TotalWorkedHours - regularMonthlyHours
                : 0;
        }

        //public async Task<List<Employee>> GetAllWithIncludeAsync(Expression<Func<Employee, bool>> predicate = null, Func<IQueryable<Employee>, IQueryable<Employee>> include = null)
        //{

        //    if (_employeeRepository == null)
        //        throw new InvalidOperationException("_repositoryForInclude is null");

        //    var result = await _employeeRepository.GetAllWithIncludeAsync(predicate, include);

        //    if (result == null)
        //        throw new InvalidOperationException("No data returned from GetAllWithIncludeAsync.");

        //    return result.ToList();
        //}

        public async Task<List<EmployeeDto>> GetAllEmployeesWithDetailsAsync()
        {

            var employees = await _employeeRepository.GetAllWithIncludeAsync(
                null,
                q => q.Include(e => e.User)
                      .ThenInclude(u => u.UserProfile)
            );

            return _mapper.Map<List<EmployeeDto>>(employees);
        }

        public async Task<List<EmployeeDto>> GetByPositionAsync(EmployeePosition position)
        {
            List<Employee> employees = (await _employeeRepository.GetAllAsync(e => e.Position == position)).ToList();
            return _mapper.Map<List<EmployeeDto>>(employees);
        }

        public async Task<List<EmployeeDto>> GetByPositionsAsync(EmployeePosition[] positions)
        {
            List<Employee> employees = (await _employeeRepository.GetAllAsync(e =>
                positions.Contains(e.Position) && e.IsActive)).ToList();

            return _mapper.Map<List<EmployeeDto>>(employees);
        }
    }
}
//_repositoryForInclude