using Project.BLL.DtoClasses;
using Project.Entities.Enums;

namespace Project.MvcUI.Areas.Admin.Models.PureVm.ResponseModel.Room
{
    public class RoomAdminResponseModel
    {
        public int Id { get; set; }
        public int FloorNumber { get; set; }
        public int RoomNumber { get; set; }
        public RoomType RoomType { get; set; }
        public RoomStatus RoomStatus { get; set; }
        public decimal Price { get; set; }
        public string? ImagePath { get; set; } // IsMain = true olan RoomImage üzerinden
        /// Wi-Fi mevcut mu?
        public bool HasWiFi { get; set; }

        /// Oda TV içeriyor mu?
        public bool HasTV { get; set; }

        /// Oda minibar içeriyor mu?
        public bool HasMinibar { get; set; }

        /// Saç kurutma makinesi var mı?
        public bool HasHairDryer { get; set; }

        /// Balkon var mı?
        public bool HasBalcony { get; set; }

        /// Oda açıklaması
        public string? Description { get; set; }

        public List<string>? ImageGallery { get; set; } // Details için Carousel desteği

        public RoomCleaningScheduleDto? CleaningInfo { get; set; }

        public RoomMaintenanceAssignmentDto? MaintenanceInfo { get; set; }
    }
}
