using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Entities.Enums
{
    public enum ComplaintStatus
    {
        Pending = 0,      // ✅ Müşteri tarafından oluşturuldu, henüz bakılmadı
        InProgress = 1,   // 🔄 İnceleme aşamasında
        Resolved = 2,     // ✅ Çözüldü ve müşteri bilgilendirildi
        Rejected = 3,     // ❌ Şikayet geçersiz/ret edildi (örn: dış kaynaklı)
        Responded = 4     // ✉️ Şikayete yanıt verildi ama çözülüp çözülmedi müşteri dönüşü bekleniyor
    }
}

