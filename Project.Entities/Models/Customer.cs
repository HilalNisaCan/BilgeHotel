using Project.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Entities.Models
{
    public class Customer:BaseEntity,IIdentifiablePerson
    {
        public int? UserId { get; set; }  // Nullable çünkü otelden gelen müşteri user olmayabilir

        public string FirstName { get; set; } = null!; // Nullable çünkü user varsa UserProfile üzerinden gelir

        public string LastName { get; set; } = null!;

        public string IdentityNumber { get; set; } = null!; // TCKN de opsiyonel olabilir


        // Sadakat puanı (ileride kampanya veya ödül sistemlerinde kullanılabilir)
        public int LoyaltyPoints { get; set; }

        // Müşteriye ait fatura bilgileri (vergi/şirket bilgileri de içerebilir)
        public string? BillingDetails { get; set; }

        // Kimlik doğrulaması başarıyla yapıldı mı?
        public bool IsIdentityVerified { get; set; }

        // Bu müşteri için kimlik kontrolü gerekiyor mu? (örneğin çocuk, yabancı misafir vs.)
        public bool NeedsIdentityCheck { get; set; }

        public string PhoneNumber { get; set; }=null!;

        //relational properties
        public virtual User User { get; set; } = null!;
        // Müşterinin rezervasyonları
        public virtual ICollection<Reservation> Reservations { get; set; } = null!;

        // Harcamalar (ekstra minibar, oda servisi, vs.)
        public virtual ICollection<ExtraExpense> ExtraExpenses { get; set; } = null!;

        // Rapor kayıtları (valilik bildirimi, Mernis eşleşmesi vb.)
        public virtual ICollection<ReportLog> ReportLogs { get; set; } = null!;

        // Müşterinin geçmişte oluşturduğu şikayet kayıtları
        public virtual ICollection<ComplaintLog> ComplaintLogs { get; set; } = null!;

        // Müşteri yanında gelen ziyaretçi kayıtları (valilik bildirimi için önemlidir)
        public virtual ICollection<GuestVisitLog> GuestVisitLogs { get; set; } = null!;

        public virtual ICollection<EarlyReservationDiscount> EarlyReservationDiscounts { get; set; } = null!;

        public virtual ICollection<Payment> Payments { get; set; } = null!;
  
    }



    /*
     * 
     * “Müşteri isterse web üzerinden kendi hesabıyla rezervasyon yapar, isterse otele gelip doğrudan resepsiyon üzerinden kayıt edilir. Bu iki farklı durumda UserId 
     * ilişkisi varsa UserProfile'dan bilgileri alıyoruz, yoksa Customer tablosundaki FirstName ve LastName alanlarını kullanıyoruz.”*/

    //“Customer tablosunu hibrit yapıya göre düzenledik. Kullanıcılar ister web üzerinden ister resepsiyon aracılığıyla sisteme girsin, müşteri verisi hem güvenli hem esnek şekilde işlenebilir.”


}
