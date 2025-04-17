using Project.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace Project.MvcUI.Models.PureVm.RequestModel.Reservation
{
    public class CreateReservationRequest
    {
        [Required(ErrorMessage = "Oda seçimi zorunludur.")]
        public int RoomId { get; set; }

        [Required(ErrorMessage = "Giriş tarihi zorunludur.")]
        [DataType(DataType.Date)]
        public DateTime CheckInDate { get; set; }

        [Required(ErrorMessage = "Çıkış tarihi zorunludur.")]
        [DataType(DataType.Date)]
        public DateTime CheckOutDate { get; set; }

        [Required(ErrorMessage = "Rezervasyon paketi zorunludur.")]
        public ReservationPackage Package { get; set; }

        [Required(ErrorMessage = "Kullanıcı bilgisi zorunludur.")]
        public string CustomerId { get; set; }
    }
}
