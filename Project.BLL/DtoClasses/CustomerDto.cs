using Project.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.DtoClasses
{
    public class CustomerDto:BaseDto,IIdentifiablePerson
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = null!;

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }=null!;

        [Required]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "TC Kimlik No 11 haneli olmalıdır.")]
        public string IdentityNumber { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public int LoyaltyPoints { get; set; }

        [StringLength(200)]
        public string BillingDetails { get; set; } = "Standart Bireysel Fatura";

        public bool IsIdentityVerified { get; set; }

        public bool NeedsIdentityCheck { get; set; }
    }
}
