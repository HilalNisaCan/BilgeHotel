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
    

        /// <summary>
        /// Şikayete verilen cevabı kaydeder.
        /// </summary>
        Task<bool> RespondToComplaintAsync(int complaintId, string response);

  
        /// <summary>
        /// Şikayet kaydını sistemden siler.
        /// </summary>
        Task<bool> DeleteComplaintAsync(int complaintId);
    }
}
