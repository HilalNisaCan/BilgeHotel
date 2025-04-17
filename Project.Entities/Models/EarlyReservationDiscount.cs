using Project.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Entities.Models
{
    public class EarlyReservationDiscount:BaseEntity
    {
        // İlgili rezervasyon (her rezervasyon en fazla bir indirim alır)
        public int ReservationId { get; set; }

        // İndirimden faydalanan müşteri
        public int CustomerId { get; set; }

        public int? CampaignId { get; set; }             // Hangi kampanya kapsamında (opsiyonel)


        // Uygulanan indirim yüzdesi
        public decimal DiscountPercentage { get; set; }

        // İndirim tipi (örn: sabit oran, özel teklif vs.)
        public DiscountType DiscountType { get; set; }

        // En az kaç gün önceden rezervasyon yapılırsa bu indirim geçerli olur?
        public int ValidityDays { get; set; }

        // İndirim ne zaman uygulandı?
        public DateTime AppliedDate { get; set; }

        //relational properties
        public virtual Reservation Reservation { get; set; } = null!; // İlgili rezervasyon bilgisi
        public virtual Customer Customer { get; set; } = null!;// İndirimden yararlanan müşteri
        public virtual Campaign Campaign { get; set; }=null!;
    }
}
