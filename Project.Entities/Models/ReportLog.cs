using Project.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Entities.Models
{
    public class ReportLog:BaseEntity
    {
        // Kullanıcı bazlı log ise UserId doludur, sistem loglarında null olabilir
        public int? UserId { get; set; }

        // Müşteri bazlı log ise CustomerId doludur, sistem hatalarında null olabilir
        public int? CustomerId { get; set; }

        public ReportType ReportType { get; set; }     // XML, Hata, Uyarı, Sistem
        public DateTime ReportDate { get; set; }       // Log zamanı
        public string? LogMessage { get; set; }         // Kısa özet
        public string? Source { get; set; }             // Örn: "ReservationModule"
        public string? ReportData { get; set; }         // JSON, XML gibi detaylı içerik
        public ReportStatus ReportStatus { get; set; }       // Başarılı / Başarısız / Bekliyor
        public string? ErrorMessage { get; set; }       // Varsa detay hata
        public string? IPAddress { get; set; }          // Kullanıcının IP adresi
        public bool IsSystemGenerated { get; set; }    // Sistem tarafından mı oluşturuldu?
        public string? XmlFilePath { get; set; }        // Varsa dışa aktarılan XML yolu

        //relational properties
        public virtual User User { get; set; } = null!;// Log kaydıyla ilişkili kullanıcı

        public virtual Customer Customer { get; set; } = null!;// Log kaydıyla ilişkili müşteri
    }
}
