using Project.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace Project.MvcUI.Models.PureVm.RequestModel.Reservation
{
    public class ReservationRequestModel
    {
        public int RoomId { get; set; }
        public string RoomType { get; set; }
        public decimal PricePerNight { get; set; }
        public decimal TotalPrice { get; set; }
        public int Duration { get; set; }
        public decimal DiscountRate { get; set; }

        [Required(ErrorMessage = "T.C. Kimlik Numarası gereklidir.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "T.C. Kimlik Numarası 11 haneli olmalıdır.")]
        public string IdentityNumber { get; set; } = null!;

        [Required]
        [DataType(DataType.Date)]
        public DateTime CheckIn { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime CheckOut { get; set; }

        [Required]
        [Range(1, 10)]
        public int GuestCount { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public ReservationPackage Package { get; set; }
    }

}
