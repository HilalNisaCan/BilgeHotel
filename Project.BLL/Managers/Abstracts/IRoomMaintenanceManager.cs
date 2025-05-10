using Project.BLL.DtoClasses;
using Project.Entities.Enums;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Managers.Abstracts
{
    /// <summary>
    /// Oda bakım/arıza işlemlerini yöneten iş akışı arayüzü.
    /// </summary>
    public interface IRoomMaintenanceManager : IManager<RoomMaintenanceDto, RoomMaintenance>
    {
        /// <summary>
        /// Eğer bugün için aynı oda ve bakım türünde kayıt varsa onu döner; yoksa yeni bir kayıt oluşturur.
        /// </summary>
        /// <param name="roomId">Bakım yapılacak odanın ID’si</param>
        /// <param name="type">Bakım tipi (örneğin: Elektrik, Su, Temizlik)</param>
        /// <returns>Var olan ya da oluşturulan bakım kaydının ID’si</returns>
        /// 
        /// <remarks>
        /// 📌 Not: Bu metot, günlük tekrar eden bakım kayıtlarını önlemek amacıyla tasarlanmıştır.  
        /// Arka planda otomasyon senaryolarında veya servis içi kontrol noktalarında kullanılabilir.  
        ///  “otomatik kontrolle bakım planlaması” örneği olarak anlatılabilir.
        /// </remarks>
        Task<int> GetOrCreateTodayMaintenanceAsync(int roomId, MaintenanceType type);
    }
}
