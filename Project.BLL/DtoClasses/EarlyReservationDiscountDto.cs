using Project.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.DtoClasses
{
    public class EarlyReservationDiscountDto:BaseDto
    {
        [Required]
        public int ReservationId { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Range(0, 100)]
        public decimal DiscountPercentage { get; set; }

        [Required]
        public DiscountType DiscountType { get; set; }

        [Range(0, 365)]
        public int ValidityDays { get; set; }

        public DateTime? AppliedDate { get; set; }
    }
}
