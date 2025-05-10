using Project.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.DtoClasses
{
    public class RoomMaintenanceAssignmentDto:BaseDto
    {
        [Required]
        public int RoomId { get; set; } // Bakımın yapılacağı oda ID'si

        [Required]
        public int MaintenanceId { get; set; } // İlişkili bakım ID'si
       
        public int RoomMaintenanceId { get; set; }

        [Required]
        public int EmployeeId { get; set; }// Görevli çalışan ID'si

        public DateTime AssignedDate { get; set; } // Atama tarihi

        public DateTime? CompletedDate { get; set; } // Tamamlanma tarihi (opsiyonel)

        public MaintenanceStatus? MaintenanceStatus { get; set; } // Bakımın güncel durumu (opsiyonel)

        public MaintenanceType MaintenanceType { get; set; }


        public string? AssignedEmployeeFullName { get; set; }
    }
}
