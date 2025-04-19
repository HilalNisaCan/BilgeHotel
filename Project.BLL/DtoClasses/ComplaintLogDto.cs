using Project.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.DtoClasses
{
    public class ComplaintLogDto:BaseDto
    {
        public int? CustomerId { get; set; }

        public string FullName { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public string Subject { get; set; } 
        public ComplaintStatus Status { get; set; } = ComplaintStatus.Pending;
        public DateTime SubmittedDate { get; set; } = DateTime.Now;
        public string Response { get; set; } = "";
        public bool IsResolved { get; set; } = false;
    }
}
