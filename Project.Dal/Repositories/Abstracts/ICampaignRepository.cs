using Project.Entities.Enums;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Dal.Repositories.Abstracts
{
    public interface ICampaignRepository:IRepository<Campaign>
    {
        Task<List<Campaign>> GetActiveCampaignsAsync(); // Aktif kampanyaları getir
        Task<bool> IsCampaignAvailableAsync(int campaignId); // Kampanya halen aktif mi?

        Task DeleteAsync(Campaign entity);
    }
}
