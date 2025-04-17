using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Project.MvcUI.Areas.Reservation.Models.PageVm
{
    public class AddExtraExpenseModel
    {
        public int ReservationId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }

        public DateTime ExpenseDate { get; set; } = DateTime.Today;

        // Dropdown'lar için
        public List<SelectListItem> CategoryList { get; set; } = new();
        public List<SelectListItem> ProductList { get; set; } = new();
    }
}
