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
    /// Şikayet kayıtlarıyla ilgili iş akışlarını tanımlar.
    /// </summary>
    public interface IComplaintLogManager : IManager<ComplaintLogDto, ComplaintLog>
    {
        /// <summary>
        /// Yeni bir şikayet kaydı oluşturur.
        /// </summary>
        Task<bool> AddComplaintAsync(int customerId, string subject, string description);

        /// <summary>
        /// Tüm şikayet kayıtlarını getirir.
        /// </summary>
        Task<List<ComplaintLogDto>> GetComplaintsAsync();

        /// <summary>
        /// Belirli müşteriye ait şikayet kayıtlarını getirir.
        /// </summary>
        Task<List<ComplaintLogDto>> GetCustomerComplaintsAsync(int customerId);

        /// <summary>
        /// Şikayete verilen cevabı kaydeder.
        /// </summary>
        Task<bool> RespondToComplaintAsync(int complaintId, string response);

        /// <summary>
        /// Şikayet durumunu (örneğin: çözüldü) günceller.
        /// </summary>
        Task<bool> UpdateComplaintStatusAsync(int complaintId, ComplaintStatus status);

        /// <summary>
        /// Şikayet kaydını sistemden siler.
        /// </summary>
        Task<bool> DeleteComplaintAsync(int complaintId);
    }
}
