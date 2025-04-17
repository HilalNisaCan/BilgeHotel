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
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Managers.Concretes
{     /// <summary>
      /// Oda bakım işlemlerinin personele atanmasını yöneten manager sınıfıdır.
      /// </summary>
    public class RoomMaintenanceAssignmentManager : BaseManager<RoomMaintenanceAssignmentDto, RoomMaintenanceAssignment>, IRoomMaintenanceAssignmentManager
    {
        private readonly IRoomMaintenanceAssignmentRepository _assignmentRepository;
        private readonly IRoomMaintenanceRepository _maintenanceRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public RoomMaintenanceAssignmentManager(
            IRoomMaintenanceAssignmentRepository assignmentRepository,
            IRoomMaintenanceRepository maintenanceRepository,
            IRoomRepository roomRepository,
            IEmployeeRepository employeeRepository,
            IMapper mapper)
            : base(assignmentRepository, mapper)
        {
            _assignmentRepository = assignmentRepository;
            _maintenanceRepository = maintenanceRepository;
            _roomRepository = roomRepository;
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Yeni bir bakım ataması oluşturur. Atama yapılmadan önce oda ve çalışan aktif mi kontrol edilir.
        /// </summary>
        public async Task<bool> AssignMaintenanceAsync(int maintenanceId, int employeeId)
        {
            var maintenance = await _maintenanceRepository.GetByIdAsync(maintenanceId);
            if (maintenance == null) return false;

            var room = await _roomRepository.GetByIdAsync(maintenance.RoomId);
            if (room == null || room.RoomStatus != RoomStatus.Available)
                return false;

            var employee = await _employeeRepository.GetByIdAsync(employeeId);
            if (employee == null || employee.IsActive == false)
                return false;

            var entity = new RoomMaintenanceAssignment
            {
                RoomMaintenanceId = maintenanceId,
                EmployeeId = employeeId,
                AssignedDate = DateTime.UtcNow,
                Status = (DataStatus)MaintenanceStatus.Pending
            };

            await _assignmentRepository.AddAsync(entity);
            return true;
        }

        /// <summary>
        /// Bakım atamasını sistemden kaldırır.
        /// </summary>
        public async Task<bool> RemoveMaintenanceAssignmentAsync(int assignmentId)
        {
            var entity = await _assignmentRepository.GetByIdAsync(assignmentId);
            if (entity == null) return false;

            await _assignmentRepository.RemoveAsync(entity);
            return true;
        }

        /// <summary>
        /// Belirli bir odaya ait tüm bakım atamalarını listeler.
        /// </summary>
        public async Task<List<RoomMaintenanceAssignmentDto>> GetRoomMaintenanceAssignmentsAsync(int roomId)
        {
            var list = await _assignmentRepository.GetAllAsync(x => x.RoomMaintenance.RoomId == roomId);
            return _mapper.Map<List<RoomMaintenanceAssignmentDto>>(list);
        }

        /// <summary>
        /// Atanmış bir bakım görevini tamamlanmış olarak işaretler.
        /// </summary>
        public async Task<bool> CompleteMaintenanceAsync(int assignmentId)
        {
            var entity = await _assignmentRepository.GetByIdAsync(assignmentId);
            if (entity == null) return false;

            entity.Status = (DataStatus)MaintenanceStatus.Completed;
            entity.CompletedDate = DateTime.UtcNow;

            await _assignmentRepository.UpdateAsync(entity);
            return true;
        }

        public async Task<bool> AssignOrCreateMaintenanceAsync(int roomId, int employeeId, DateTime date, string? description)
        {// Aynı gün aynı odaya bakım var mı?
            RoomMaintenance? existing = await _maintenanceRepository.GetFirstOrDefaultAsync(
                predicate: x => x.RoomId == roomId &&
                                x.ScheduledDate.Date == date.Date &&
                                x.Status != DataStatus.Deleted,
                include: q => q // include boş verildi ama parametre karşılandı ✅
            );

            if (existing == null)
            {
                existing = new RoomMaintenance
                {
                    RoomId = roomId,
                    ScheduledDate = date,
                    MaintenanceType = MaintenanceType.GeneralMaintenance, // varsayılan veya dışarıdan al
                    StartDate = DateTime.MinValue,
                    MaintenanceStatus = MaintenanceStatus.Scheduled,
                    Description = "Görev atamasıyla oluşturuldu",
                    CreatedDate = DateTime.Now,
                    Status = DataStatus.Inserted
                };

                await _maintenanceRepository.CreateAsync(existing);
            }

            RoomMaintenanceAssignment assignment = new RoomMaintenanceAssignment
            {
                RoomId = roomId,
                RoomMaintenanceId = existing.Id,
                EmployeeId = employeeId,
                AssignedDate = DateTime.Now,
                MaintenanceStatus = MaintenanceStatus.Scheduled,
                Description = description,
                Status = DataStatus.Inserted,
                CreatedDate = DateTime.Now
            };

            await _assignmentRepository.CreateAsync(assignment);
            return true;
        }

        public async Task<RoomMaintenanceAssignmentDto?> GetLatestByRoomIdAsync(int roomId)
        {
            List<RoomMaintenanceAssignment> list = (await _assignmentRepository.GetAllWithIncludeAsync(
                x => x.RoomId == roomId && x.Status != DataStatus.Deleted,
                q => q.Include(x => x.Employee)
            )).ToList();

            RoomMaintenanceAssignment? latest = list
                .OrderByDescending(x => x.AssignedDate)
                .FirstOrDefault();

            return latest == null
                ? null
                : _mapper.Map<RoomMaintenanceAssignmentDto>(latest);
        }

        public async Task<bool> CreateWithEntityAsync(RoomMaintenanceAssignment entity)
        {
            await _assignmentRepository.AddAsync(entity);
            return true; // SaveChangesAsync çağrısını AddAsync içinde yapıyoruz
        }
    }
}
