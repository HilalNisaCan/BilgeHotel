using Project.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Entities.Interfaces
{
    /// Tüm tabloların ortak özelliklerini taşıyan interface.

    public interface IEntity
    {
        public int Id { get; set; }                // Benzersiz kimlik (Primary Key)
        public DateTime CreatedDate { get; set; }  // Kaydın oluşturulma tarihi
        public DateTime? ModifiedDate { get; set; } // Kaydın güncellenme tarihi (Nullable)
        public DateTime? DeletedDate { get; set; } // Kaydın silinme tarihi (Nullable)
        public DataStatus Status { get; set; } // Kaydın durumu (Active, Updated, Deleted)
    }
}
