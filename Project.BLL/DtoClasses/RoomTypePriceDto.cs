using Project.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.DtoClasses
{
    /// <summary>
    /// Oda tipi ve gecelik fiyat bilgisini taşıyan veri transfer nesnesi.
    /// </summary>
    public class RoomTypePriceDto:BaseDto
    {
        public int Id { get; set; }

        /// <summary>
        /// Odanın tipi (Enum: Single, Double, Suite vs.)
        /// </summary>
        public RoomType RoomType { get; set; }

        /// <summary>
        /// Gecelik konaklama fiyatı
        /// </summary>
        public decimal PricePerNight { get; set; }
    }
}
