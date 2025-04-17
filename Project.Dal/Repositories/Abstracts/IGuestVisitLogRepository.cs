using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Dal.Repositories.Abstracts
{
    public interface IGuestVisitLogRepository:IRepository<GuestVisitLog>
    {
        Task<List<GuestVisitLog>> GetVisitsByCustomerIdAsync(int customerId); // Belirli bir müşterinin ziyaret kayıtlarını getir
        Task<List<GuestVisitLog>> GetVisitsByDateAsync(DateTime date); // Belirli bir tarihte yapılan ziyaretleri getir
        Task<List<GuestVisitLog>> GetRecentVisitsAsync(int count); // Son X ziyaret kaydını getir
    }
}
