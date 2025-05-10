using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Project.Entities.Models;
using Project.MvcUI.Areas.Admin.Models.PureVm.RequestModel.Campaign;
using Project.MvcUI.Areas.Admin.Models.PureVm.ResponseModel.Campaign;
using Project.BLL.Managers;
using Project.BLL.Managers.Abstracts;
using Project.BLL.DtoClasses;


namespace Project.MvcUI.Areas.Admin.Controllers
{
    /*“CampaignController, otelde tanımlanan kampanyaların listeleme, ekleme, düzenleme ve silme işlemlerini yöneten yönetici paneli controller’ıdır.
Yapı, katmanlı mimariye uygun olarak tasarlanmıştır.
Kullanıcıdan gelen veriler CampaignRequestModel aracılığıyla alınır ve AutoMapper ile CampaignDto'ya dönüştürülerek iş katmanına (BLL) iletilir.
Listeleme işlemleri için CampaignDto nesneleri, CampaignResponseModel'e dönüştürülerek View'a aktarılır.
Bu yapı sayesinde ViewModel, DTO ve Entity’ler arasında net bir ayrım sağlanmış olur.
Ayrıca hata yönetimi ModelState kontrolleriyle sağlanır, tüm veri transferi AutoMapper ile gerçekleştirilerek temiz ve sürdürülebilir bir yapı oluşturulmuştur.”*/

    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class CampaignController : Controller
    {
        private readonly ICampaignManager _campaignManager;
        private readonly IMapper _mapper;

        public CampaignController(  ICampaignManager campaignManager, IMapper mapper)
        {
            _campaignManager = campaignManager;
            _mapper = mapper;
        }

        /// <summary>
        /// Tüm kampanyaları listeler (DTO → ResponseModel dönüşümü yapılır)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<CampaignDto> dtoList = await _campaignManager.GetAllAsync();
            List<CampaignResponseModel> modelList = _mapper.Map<List<CampaignResponseModel>>(dtoList);
            return View(modelList);
        }

        /// <summary>
        /// Yeni kampanya oluşturma formu
        /// </summary>
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Yeni kampanya oluşturur (RequestModel → DTO dönüşümü yapılır)
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create(CampaignRequestModel model)
        {
            if (!ModelState.IsValid) return View(model);

            CampaignDto dto = _mapper.Map<CampaignDto>(model);

            // ✅ Kampanya oluşturuluyor ve geri dönüyor
            Campaign createdCampaign = await _campaignManager.CreateAndReturnAsync(dto);

            // 📩 E-posta yalnızca aktif kampanyalar için gönderiliyor
            await _campaignManager.NotifyUsersAsync(createdCampaign);

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Kampanya düzenleme formu (DTO → RequestModel dönüşümü)
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            CampaignDto dto = await _campaignManager.GetByIdAsync(id);
            if (dto == null) return NotFound();

            CampaignRequestModel model = _mapper.Map<CampaignRequestModel>(dto);
            return View(model);
        }

        /// <summary>
        /// Kampanya bilgilerini günceller (RequestModel → DTO dönüşümü)
        /// </summary>
        [HttpPost("{id}")]
        public async Task<IActionResult> Edit(int id, CampaignRequestModel model)
        {
            if (!ModelState.IsValid) return View(model);

            CampaignDto dto = _mapper.Map<CampaignDto>(model);
            dto.Id = id;

            await _campaignManager.UpdateAsync(dto);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Kampanya silme işlemi
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            CampaignDto dto = await _campaignManager.GetByIdAsync(id);
            if (dto == null) return NotFound();

            await _campaignManager.DeleteAsync(dto);
            return RedirectToAction("Index");
        }
    }
}