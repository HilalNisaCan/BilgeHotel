using Project.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Entities.Models
{
    public class BackupLog:BaseEntity
    {

        public int UserId { get; set; } // Yedeklemeyi yapan kişi
        public bool IsAuthorized { get; set; } // Yetkili mi?

        public string? FilePath { get; set; } // Yedek dosyasının yolu
        public DateTime BackupDate { get; set; } // Ne zaman yedek alındı?
        public BackupStatus BackupStatus { get; set; } // İşlem sonucu
        public bool IsRestored { get; set; } // Geri yüklendi mi?

        public DateTime? RestoredDate { get; set; } // Geri yükleme tarihi (varsa)
        public string? ErrorMessage { get; set; } // Hata mesajı (opsiyonel)
        public string? IPAddress { get; set; } // IP adresi (audit trail için)
        public string? MachineName { get; set; } // Cihaz adı (log denetimi için)

        //relational properties
        [ForeignKey("UserId")] // ✅ Bu şart
        public virtual User User { get; set; } = null!;
       
    }
}
