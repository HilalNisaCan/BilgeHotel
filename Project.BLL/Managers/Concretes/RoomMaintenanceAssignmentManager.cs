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
        private readonly IMapper _mapper;

        public RoomMaintenanceAssignmentManager(
            IRoomMaintenanceAssignmentRepository assignmentRepository,
         
            IMapper mapper)
            : base(assignmentRepository, mapper)
        {
            _assignmentRepository = assignmentRepository;
            _mapper = mapper;
        }



        /// <summary>
        /// Belirtilen odaya ait en son bakım atamasını getirir.
        /// </summary>
        /// <param name="roomId">Bakımı yapılacak odanın ID’si</param>
        /// <returns>Son bakım ataması varsa DTO olarak döner, yoksa null</returns>
        /// 
        /// <remarks>
        /// 📌 Not: Bu metot, bir odada daha önce yapılmış son bakım görevlendirmesini bulmak için kullanılır.  
        /// Atamalar tarihe göre sıralanır ve en güncel olan geri döndürülür.  
        /// Kullanıcıya bakım geçmişinin özeti veya son atanan personel bilgisi gösterilirken kullanılabilir.
        /// </remarks>
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


    }
}
