using Project.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Entities.Models
{
    public class Review:BaseEntity
    {

        public int UserId { get; set; }              // Yorumu yapan kullanıcı
      
        public RoomType RoomType { get; set; }
        public int Rating { get; set; }              // Puan (1-5) – kontrol BLL katmanında yapılacak
        public string? Comment { get; set; }          // Yorum içeriği
        public DateTime CommentDate { get; set; }    // Yorum tarihi

        public bool IsApproved { get; set; }         // Yorum yönetici tarafından onaylandı mı?
        public bool IsAnonymous { get; set; }        // 💡 Anonim yorum mu? (Ad-soyad gösterilmeyebilir)

        // 🔗 Navigation
        public virtual User User { get; set; } = null!;
       
    }
}
