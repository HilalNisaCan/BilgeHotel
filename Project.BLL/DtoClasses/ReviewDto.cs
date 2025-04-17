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

        [Required]
        public int ReservationId { get; set; } // Yoruma konu olan rezervasyon ID

        [Required]
        public int RoomId { get; set; } // Yorum yapılan oda ID

        [Required, StringLength(1000)]
        public string Comment { get; set; } // Yorum metni

        [Range(1, 5)]
        public int Rating { get; set; } // Puan (1-5 arası)

        public DateTime CommentDate { get; set; } // Yorum tarihi

        public bool IsApproved { get; set; } // Yorum onaylandı mı?
    }
}
