using Microsoft.AspNetCore.Identity;
using Project.Entities.Enums;
using Project.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Entities.Models
{
    /// <summary>
    /// Uygulamadaki rollerin temsil edildiği özelleştirilmiş rol sınıfı.
    /// </summary>
    public class AppRole : IdentityRole<int>,IEntity
    {
        /// <summary>
        /// Rolün açıklaması (örn: Yöneticiler için tüm yetkiler açık vs.)
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Rolün oluşturulduğu tarih
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? ModifiedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public DataStatus Status { get; set; }

    

        // Navigation property: Bu role ait kullanıcılar
        public virtual ICollection<User> Users { get; set; } = new List<User>();
     
    }
}
