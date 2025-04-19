using Project.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Entities.Models
{
    public class ComplaintLog:BaseEntity
    {
        public int? CustomerId { get; set; }    
        public string? FullName { get; set; }        // Zorunlu
        public string?Email { get; set; }           // Zorunlu
        public string? Description { get; set; }     // Zorunlu
        public string Subject { get; set; }
        // Sistem tarafından otomatik atanacak alanlar
        public ComplaintStatus ComplaintStatus { get; set; } = ComplaintStatus.Pending;
        public DateTime SubmittedDate { get; set; } = DateTime.Now;
        public string Response { get; set; } = "";
        public bool IsResolved { get; set; } = false;

        //relatioanal properties
        public virtual Customer Customer { get; set; } 
    }
}
