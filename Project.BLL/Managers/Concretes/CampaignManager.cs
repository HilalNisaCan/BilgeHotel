using AutoMapper;
using Project.BLL.DtoClasses;
using Project.BLL.Managers.Abstracts;
using Project.Dal.Repositories.Abstracts;
using Project.Dal.Repositories.Concretes;
using Project.Entities.Enums;
using Project.Entities.Models;
using Project.Common.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Managers.Concretes
{
    public class CampaignManager : BaseManager<CampaignDto, Campaign>, ICampaignManager
    {
        private readonly ICampaignRepository _campaignRepository;
        private readonly IUserRepository _userRepository;
       
        private readonly IMapper _mapper;

        public CampaignManager(ICampaignRepository campaignRepository, IMapper mapper, IUserRepository userRepository)
            : base(campaignRepository, mapper)
        {
            _campaignRepository = campaignRepository;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<Campaign> CreateAndReturnAsync(CampaignDto dto)
        {
            Campaign entity = _mapper.Map<Campaign>(dto);
            entity.CreatedDate = DateTime.Now;
            entity.Status = DataStatus.Inserted;

            await _campaignRepository.CreateAsync(entity);
            return entity;
        }


        /// <summary>
        /// Kampanyayı DTO üzerinden siler.
        /// </summary>
        public async Task DeleteAsync(CampaignDto dto)
        {
            Campaign? entity = await _campaignRepository.GetByIdAsync(dto.Id);
            if (entity == null) return;

            await _campaignRepository.DeleteAsync(entity);
        }

        /// <summary>
        /// Kampanyayı ID üzerinden siler.
        /// </summary>
        public async Task<bool> DeleteCampaignAsync(int campaignId)
        {
            Campaign? campaign = await _campaignRepository.GetByIdAsync(campaignId);
            if (campaign == null)
                return false;

            await _campaignRepository.RemoveAsync(campaign);
            return true;
        }

        public async Task<int?> MatchCampaignAsync(DateTime checkIn, ReservationPackage package)
        {
            int daysBefore = (checkIn - DateTime.Today).Days;

            List<Campaign> campaigns = (await _campaignRepository.GetAllAsync()).ToList();

            if (daysBefore >= 90)
                return campaigns.FirstOrDefault(c => c.DiscountPercentage == 23)?.Id;

            if (daysBefore >= 30 && package == ReservationPackage.AllInclusive)
                return campaigns.FirstOrDefault(c => c.DiscountPercentage == 18 && c.Package == ReservationPackage.AllInclusive)?.Id;

            if (daysBefore >= 30 && package == ReservationPackage.Fullboard)
                return campaigns.FirstOrDefault(c => c.DiscountPercentage == 16 && c.Package == ReservationPackage.Fullboard)?.Id;

            return null;
        }

        public async Task NotifyUsersAsync(Campaign campaign)
        {
            Console.WriteLine("🔔 NotifyUsersAsync METODU ÇAĞRILDI");
            Console.WriteLine($"🎯 Kampanya aktif mi? {campaign.IsActive}");
            Console.WriteLine($"📅 Tarih aralığı: {campaign.StartDate:dd.MM.yyyy} - {campaign.EndDate:dd.MM.yyyy}");

            if (!campaign.IsActive)
            {
                Console.WriteLine("❌ Kampanya pasif, e-posta gönderilmedi.");
                return;
            }

            // Şu hale getir:
            if (campaign.EndDate < DateTime.Today)
                return;

            List<User> users = (await _userRepository.GetAllAsync()).ToList();
            List<User> recipients = users
                .Where(u => u.WantsCampaignEmails && !string.IsNullOrWhiteSpace(u.Email))
                .ToList();

            Console.WriteLine($"📬 E-posta gönderilecek kullanıcı sayısı: {recipients.Count}");

            string subject = "🎉 Yeni Kampanya Sizi Bekliyor!";
            string formattedDate = $"{campaign.StartDate:dd.MM.yyyy} - {campaign.EndDate:dd.MM.yyyy}";

            foreach (User user in recipients)
            {
                string body = $@"
            <h3>Merhaba {user.UserName},</h3>
            <p>Yeni bir kampanyamız var!</p>
    <p><strong>{campaign.Name}</strong> adlı  yeni kampanyamız yayında!</p>
            <ul>
                <li><strong>Paket:</strong> {campaign.Package}</li>
                <li><strong>İndirim:</strong> %{campaign.DiscountPercentage}</li>
                <li><strong>Tarih:</strong> {formattedDate}</li>
            </ul>
            <p>Hemen rezervasyon yapmayı unutma!</p>";

                bool success = EmailService.Send(user.Email, body, subject);
                Console.WriteLine(success
                    ? $"📩 Mail gönderildi: {user.Email}"
                    : $"❌ Mail gönderilemedi: {user.Email}");
            }
        }
    }

}
