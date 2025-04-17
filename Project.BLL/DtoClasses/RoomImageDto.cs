using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.DtoClasses
{
    public class RoomImageDto : BaseDto
    {
        [Required]
        public int RoomId { get; set; } // Bu fotoğrafın ait olduğu oda ID'si

        [Required, StringLength(200)]
        public string ImagePath { get; set; }// Fotoğrafın dosya yolu veya URL'si
        public bool IsMain { get; set; }
    }
}
