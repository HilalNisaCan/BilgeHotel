using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Entities.Models
{
    public class RoomImage:BaseEntity
    {

        // Hangi odaya ait?
        public int RoomId { get; set; }

        // Görselin dosya yolu veya URL'si (UI'da doğrudan gösterim için)
        public string? ImagePath { get; set; }

        // ✅ Ana görsel mi? (true: Kapak görseli, false: Galeri görseli)
        public bool IsMain { get; set; }

        //relational properties
        public virtual Room Room { get; set; } = null!;    // Görselin ait olduğu oda
    }
}
