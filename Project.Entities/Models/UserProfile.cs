using Project.Entities.Enums;
using Project.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Entities.Models
{
    public class UserProfile:BaseEntity ,IIdentifiablePerson
    {
        // Kullanıcının adı
        public string FirstName { get; set; } = null!;

        // Kullanıcının soyadı
        public string LastName { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        // Genel adres bilgisi
        public string? Address { get; set; }

        // Kullanıcının yaşadığı şehir (raporlama, istatistik için kullanılabilir)
        public string? City { get; set; }

        // Ülke bilgisi (özellikle yabancı müşteriler için valilik bildirimi gerekebilir)
        public string? Country { get; set; }

        // Uyruk bilgisi (MERNİS doğrulama ve resmi raporlar için gereklidir)
        public string? Nationality { get; set; }

        // Cinsiyet bilgisi (opsiyonel, istatistiksel veri için)
        public string? Gender { get; set; }

        // T.C. Kimlik Numarası (kimlik doğrulama ve fatura işlemleri için gerekli olabilir)
        public string IdentityNumber { get; set; } = null!;

        // Doğum tarihi (yaş aralığına göre işlem yapmak için)
        public DateTime? BirthDate { get; set; }

        public string? ProfileImagePath { get; set; }

        // Kullanıcı ile birebir ilişki
        public int UserId { get; set; }

        //Relational Properties
        public virtual User User { get; set; } = null!;
       




        //Not;“Biz kullanıcıyla ilgili kimlik ve profil detaylarını asıl UserProfile tablosunda tutuyoruz. Bu yapı modülerlik sağlıyor ve ASP.NET Identity ile karışıklığı önlüyor.”
    }
}
