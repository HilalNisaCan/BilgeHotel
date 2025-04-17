using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Dal.Repositories.Abstracts
{
    public interface IComplaintLogRepository:IRepository<ComplaintLog>
    {
        Task<List<ComplaintLog>> GetUnresolvedComplaintsAsync(); // Çözümlenmemiş şikayetleri getir
        Task<List<ComplaintLog>> GetResolvedComplaintsAsync(); // Çözümlenmiş şikayetleri getir
        Task<List<ComplaintLog>> GetComplaintsByCustomerIdAsync(int customerId); // Belirli bir müşteriye ait şikayetleri getir
        Task<ComplaintLog> GetLatestComplaintByCustomerIdAsync(int customerId); // Müşterinin en son şikayetini getir
        Task DeleteAsync(int id); // ✅ Şikayet silme işlemi 
    }
}
