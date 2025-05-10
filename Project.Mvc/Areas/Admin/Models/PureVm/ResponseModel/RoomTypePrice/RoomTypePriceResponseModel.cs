using Project.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace Project.MvcUI.Areas.Admin.Models.PureVm.ResponseModel.RoomTypePrice
{
    /// <summary>
    /// Oda tipi fiyatının kullanıcıya gösterilecek versiyonu (string tip açıklama içerir).
    /// </summary>
    public class RoomTypePriceResponseModel
    {
        public int Id { get; set; }

        public RoomType RoomType { get; set; }  // Enum (veritabanı işlemleri için)

        public string RoomTypeName { get; set; } = string.Empty; // Görselde gösterim için (ToString)

        [Range(1, 100000, ErrorMessage = "Fiyat 1₺ ile 100000₺ arasında olmalıdır.")]
        public decimal PricePerNight { get; set; }
    }
}
