using Project.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.DtoClasses
{
    public class BackupLogDto:BaseDto
    {
        [Required]
        public int UserId { get; set; } // Yedeği alan kullanıcının ID'si

        public bool IsAuthorized { get; set; } // Kullanıcı yedekleme için yetkili mi?

        [Required, StringLength(300)]
        public string? FilePath { get; set; } // Yedek dosyasının yolu

        public DateTime BackupDate { get; set; } // Yedekleme işleminin tarihi

        public bool IsRestored { get; set; } // Yedekten geri yükleme yapıldı mı?

        public BackupStatus BackupStatus { get; set; }  // Yedekleme durumu (Başarılı, Başarısız)

        public DateTime? RestoredDate { get; set; }
        public string? ErrorMessage { get; set; }
        public string? IPAddress { get; set; }
        public string? MachineName { get; set; }
        public string? UserFullName { get; set; }
    }
}
