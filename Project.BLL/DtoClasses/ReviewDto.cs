using Project.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.DtoClasses
{
    public class ReviewDto:BaseDto
    {
        [Required]
        public int UserId { get; set; } // Yorumu yapan kullanıcının ID'si

    
        public int ReservationId { get; set; } // Yoruma konu olan rezervasyon ID

       public RoomType RoomType { get; set; }


        [Required, StringLength(1000)]
        public string Comment { get; set; } // Yorum metni

        [Range(1, 5)]
        public int Rating { get; set; } // Puan (1-5 arası)

        public DateTime CommentDate { get; set; } // Yorum tarihi

        public bool IsApproved { get; set; } // Yorum onaylandı mı?

        public bool IsAnonymous { get; set; }

        public string? UserFirstName { get; set; }
        public string? UserLastName { get; set; }
        public string? UserEmail { get; set; }
    }
}
