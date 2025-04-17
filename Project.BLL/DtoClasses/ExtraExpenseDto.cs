using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.DtoClasses
{
    public class ExtraExpenseDto:BaseDto
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public int ReservationId { get; set; }

        public int ProductId { get; set; }

        public int? PaymentId { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public DateTime? ExpenseDate { get; set; }

        public string Description { get; set; } = null!;

        // 🔁 Navigation için isimler (Response tarafında işlenir)
        public string CustomerFullName { get; set; } = null!;

        public string ProductName { get; set; } = null!;

        public string ReservationInfo { get; set; } = null!;
    }
}
