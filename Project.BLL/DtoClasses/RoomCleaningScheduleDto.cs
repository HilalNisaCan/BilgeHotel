using Project.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.DtoClasses
{
    public class RoomCleaningScheduleDto:BaseDto
    {
        public int RoomId { get; set; }
        public DateTime ScheduledDate { get; set; }
        public CleaningStatus CleaningStatus { get; set; }
        public string? Description { get; set; }
        public int AssignedEmployeeId { get; set; }
        public RoomStatus RoomStatus { get; set; }
        public bool IsCompleted { get; set; }
        public string? AssignedEmployeeFullName { get; set; }
    }
}
