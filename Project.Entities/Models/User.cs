using Microsoft.AspNetCore.Identity;
using Project.Entities.Enums;
using Project.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Entities.Models
{
    // `IdentityUser<int>` sınıfından miras alarak ASP.NET Identity altyapısını kullanır
    // `IEntity` arayüzü ile temel varlık özelliklerini içerir
    public class User : IdentityUser<int>, IEntity
    {
        // Kullanıcı aktivasyon işlemleri için bir kod tutar
        public Guid? ActivationCode { get; set; }

        // Kullanıcının hesabının aktif olup olmadığını belirler
        public bool IsActivated { get; set; }

        // Kullanıcının sistemdeki rolünü belirler (Müşteri, Resepsiyonist, Yönetici)
        public UserRole Role { get; set; }

        // Kullanıcının sisteme kayıt olduğu tarih
        public DateTime CreatedDate { get; set; }

        // Kullanıcı bilgileri güncellendiğinde değişiklik tarihi tutulur
        public DateTime? ModifiedDate { get; set; }

        // Kullanıcı sistemden silindiğinde bu tarih kaydedilir
        public DateTime? DeletedDate { get; set; }

        // Kullanıcının aktif, pasif veya silinmiş olup olmadığını belirler
        public DataStatus Status { get; set; }

        //Relational properties

        // Kullanıcıya ait profil bilgilerini saklayan ilişki
        public virtual UserProfile UserProfile { get; set; } = null!;

        public virtual Customer Customer { get; set; } = null!;

        public virtual Employee Employee { get; set; } = null!;


        // Kullanıcının yaptığı rezervasyonları tutar
        public virtual ICollection<Reservation> Reservations { get; set; } = null!;
            

        // Kullanıcının oluşturduğu siparişleri tutar
        public virtual ICollection<Order> Orders { get; set; } = null!;

        // Kullanıcının otel ile ilgili yaptığı yorumları tutar
        public virtual ICollection<Review> Reviews { get; set; } = null!;

        // Kullanıcının yaptığı ödemeleri takip eder
        public virtual ICollection<Payment> Payments { get; set; } = null!;

        // Kullanıcının sistem üzerinde yaptığı işlemlerin loglarını saklar
        public virtual ICollection<ReportLog> ReportLogs { get; set; } = null!;

        //Kullanıcının sistem üzerinde yaptığı Yedekleme işlemlerin loglarını saklar
        public virtual ICollection<BackupLog> BackupLogs { get; set; } = null!;
    }
}
