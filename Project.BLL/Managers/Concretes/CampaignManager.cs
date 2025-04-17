using AutoMapper;
using Project.BLL.DtoClasses;
using Project.BLL.Managers.Abstracts;
using Project.Dal.Repositories.Abstracts;
using Project.Dal.Repositories.Concretes;
using Project.Entities.Enums;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Managers.Concretes
{
    public class CampaignManager : BaseManager<CampaignDto, Campaign>, ICampainManager
    {
        private readonly ICampaignRepository _campaignRepository;
        private readonly IMapper _mapper;

        public CampaignManager(ICampaignRepository campaignRepository, IMapper mapper)
            : base(campaignRepository, mapper)
        {
            _campaignRepository = campaignRepository;
            _mapper = mapper;
        }

        public async Task<int> CreateCampaignAsync(CampaignDto campaignDto)
        {
            var campaign = _mapper.Map<Campaign>(campaignDto);
            campaign.IsActive = true; // ✅ Varsayılan olarak kampanya aktif eklenir
            campaign.CreatedDate = DateTime.UtcNow;

            await _campaignRepository.AddAsync(campaign);
            return campaign.Id;
        }

        public async Task DeleteAsync(CampaignDto dto)
        {
            var entity = await _campaignRepository.GetByIdAsync(dto.Id); // EF'in zaten takip ettiği entity'yi al
            if (entity == null) return;

            await _campaignRepository.DeleteAsync(entity);
        }

        public async Task<bool> DeleteCampaignAsync(int campaignId)
        {
            var campaign = await _campaignRepository.GetByIdAsync(campaignId);
            if (campaign == null)
                return false;

            await _campaignRepository.RemoveAsync(campaign);
            return true;
        }

        public async Task<List<CampaignDto>> GetActiveCampaignsAsync()
        {
            var activeCampaigns = await _campaignRepository.GetAllAsync(c => c.IsActive && c.EndDate > DateTime.UtcNow);
            return _mapper.Map<List<CampaignDto>>(activeCampaigns);
        }

        public async Task<CampaignDto> GetCampaignByIdAsync(int campaignId)
        {
            var campaign = await _campaignRepository.GetByIdAsync(campaignId);
            return campaign == null ? null : _mapper.Map<CampaignDto>(campaign);
        }

        public async Task<bool> UpdateCampaignAsync(int campaignId, CampaignDto campaignDto)
        {
            var existingCampaign = await _campaignRepository.GetByIdAsync(campaignId);
            if (existingCampaign == null)
                return false;

            _mapper.Map(campaignDto, existingCampaign);
            await _campaignRepository.UpdateAsync(existingCampaign);

            return true;
        }
    }

}
