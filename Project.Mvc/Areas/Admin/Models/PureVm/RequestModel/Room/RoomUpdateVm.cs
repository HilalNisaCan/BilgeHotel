using Project.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace Project.MvcUI.Areas.Admin.Models.PureVm.RequestModel.Room
{
    public class RoomUpdateVm
    {
        public int Id { get; set; }

        [Display(Name = "Oda Numarası")]
        [Required(ErrorMessage = "Oda numarası zorunludur.")]
        public int RoomNumber { get; set; }

        [Display(Name = "Kat Numarası")]
        [Required(ErrorMessage = "Kat numarası zorunludur.")]
        public int FloorNumber { get; set; }

        [Display(Name = "Oda Tipi")]
        [Required(ErrorMessage = "Oda tipi seçilmelidir.")]
        public RoomType RoomType { get; set; }

        [Display(Name = "Durum")]
        [Required(ErrorMessage = "Oda durumu seçilmelidir.")]
        public RoomStatus Status { get; set; }

        [Display(Name = "Açıklama")]
        [MaxLength(1000)]
        public string? Description { get; set; }

        [Display(Name = "Minibar")]
        public bool HasMinibar { get; set; }

        [Display(Name = "Saç Kurutma Makinesi")]
        public bool HasHairDryer { get; set; }

        [Display(Name = "TV")]
        public bool HasTV { get; set; }

        [Display(Name = "Kablosuz İnternet")]
        public bool HasWiFi { get; set; }

        [Display(Name = "Balkon")]
        public bool HasBalcony { get; set; }

        [Display(Name = "Klima")]
        public bool HasAirConditioning { get; set; }
    }
}
