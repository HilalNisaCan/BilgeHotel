using Project.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.DtoClasses
{
    public class ReportLogDto:BaseDto
    {
        public int? UserId { get; set; } // Logu oluşturan kullanıcı (nullable, sistem logları olabilir)

        [Required]
        public int CustomerId { get; set; } // Logun ait olduğu müşteri

        [Required]
        public ReportType ReportType { get; set; } // Log tipi: XML, sistem hatası vb.

        public DateTime? ReportDate { get; set; } // Log oluşturulma tarihi

        public string? LogMessage { get; set; } // Kısa açıklama

        public string? Source { get; set; } // Kaynak (örneğin: ödeme sistemi, rezervasyon)

        public string? ReportData { get; set; } // Log içeriği (örn: JSON veya XML)

        public ReportStatus ReportStatus { get; set; } // Başarılı/Başarısız vb.

        public string? ErrorMessage { get; set; } // Varsa hata açıklaması

        public string? IPAddress { get; set; } // Logun alındığı IP adresi

        public bool IsSystemGenerated { get; set; } // Sistem tarafından mı üretildi?

        public string? XmlFilePath { get; set; } // Dışa aktarılan XML dosyasının yolu
    }
}
