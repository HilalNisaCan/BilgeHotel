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
    /// Kampanya işlemlerini tanımlar.
    /// </summary>
    public interface ICampaignManager : IManager<CampaignDto, Campaign>
    {
        /// <summary>
        /// Kampanyayı ID ile siler.
        /// </summary>
        Task<bool> DeleteCampaignAsync(int campaignId);

        /// <summary>
        /// Kampanyayı DTO ile siler.
        /// </summary>
        Task DeleteAsync(CampaignDto dto);

        Task<int?> MatchCampaignAsync(DateTime checkIn, ReservationPackage package);

        Task NotifyUsersAsync(Campaign campaign);

        Task<Campaign> CreateAndReturnAsync(CampaignDto dto);
    }
}
