using Project.BLL.DtoClasses;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Managers.Abstracts
{
    /// <summary>
    /// Oda bakım işlemlerinin personele atanmasıyla ilgili iş akışlarını tanımlar.
    /// </summary>
    public interface IRoomMaintenanceAssignmentManager : IManager<RoomMaintenanceAssignmentDto, RoomMaintenanceAssignment>
    {

        /// <summary>
        /// Verilen odaya ait en son bakım atamasını döndürür (son atama tarihi baz alınır).
        /// </summary>
        Task<RoomMaintenanceAssignmentDto?> GetLatestByRoomIdAsync(int roomId);
     
    }
}
