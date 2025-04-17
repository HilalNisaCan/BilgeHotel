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
    public interface ICampainManager : IManager<CampaignDto, Campaign>
    {
        /// <summary>
        /// Yeni kampanya oluşturur ve sistemde aktif olarak ekler.
        /// </summary>
        Task<int> CreateCampaignAsync(CampaignDto campaignDto); // ✅ Yeni kampanya ekle
        /// <summary>
        /// Kampanyayı ID'ye göre günceller.
        /// </summary>
        Task<bool> UpdateCampaignAsync(int campaignId, CampaignDto campaignDto); // ✅ Kampanyayı güncelle
        /// <summary>
        /// Sadece şu an aktif olan kampanyaları döner.
        /// </summary>
        Task<List<CampaignDto>> GetActiveCampaignsAsync(); // ✅ Aktif kampanyaları getir
        /// <summary>
        /// Kampanyayı ID'ye göre getirir.
        /// </summary>
        Task<CampaignDto> GetCampaignByIdAsync(int campaignId); // ✅ Kampanyayı ID'ye göre getir
        /// <summary>
        /// Kampanyayı ID'ye göre siler (kalıcı silme).
        /// </summary>
        Task<bool> DeleteCampaignAsync(int campaignId); // ✅ Kampanyayı sil

        Task DeleteAsync(CampaignDto dto);
    }
}
