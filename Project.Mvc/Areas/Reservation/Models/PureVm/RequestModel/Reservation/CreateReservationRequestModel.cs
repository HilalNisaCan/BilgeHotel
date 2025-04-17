using Microsoft.AspNetCore.Mvc.Rendering;
using Project.Entities.Enums;
using Project.Entities.Models;
using Project.MvcUI.Models.PureVm.ResponseModel.Room;
using System.ComponentModel.DataAnnotations;

namespace Project.MvcUI.Areas.Reservation.Models.PureVm.RequestModel.Reservation
{
    public class CreateReservationRequestModel
    {
        [Required]
        public string FirstName { get; set; } = null!;

        [Required]
        public string LastName { get; set; } = null!;

        [Required]
        [StringLength(11, MinimumLength = 11)]
        public string IdentityNumber { get; set; } = null!;

        [Required]
        public int BirthYear { get; set; }

        [Required]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        public string BillingDetails { get; set; } = "Standart Bireysel Fatura";

        [Required]
        public int RoomId { get; set; }

        [Required]
        public DateTime CheckIn { get; set; }

        [Required]
        public DateTime CheckOut { get; set; }

        [Required]
        [Range(0.01, 100000)]
        public decimal TotalPrice { get; set; }

        [Required]
        [Range(1, 365)]
        public int Duration { get; set; }

        [Required]
        [Range(0, 1)]
        public double DiscountRate { get; set; }

        [Required]
        public int GuestCount { get; set; }

        public ReservationPackage Package { get; set; }

        public List<RoomResponseModel> RoomList { get; set; } = new();
    }

}
