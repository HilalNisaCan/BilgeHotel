using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Project.BLL.DtoClasses;
using Project.BLL.Managers.Abstracts;
using Project.MvcUI.Areas.Admin.Models.PureVm.RequestModel.ExcahangeRate;
using Project.MvcUI.Areas.Admin.Models.PureVm.ResponseModel.ExcahangeRate;

namespace Project.MvcUI.Areas.Admin.Controllers
{
    /// <summary>
    /// 💱 ExchangeRateController
    /// 
    /// Yöneticilerin döviz kuru giriş ve yönetimini sağladığı admin controller'dır.
    /// Kurlar sistemde hem listelenebilir hem de Create/Update/Delete işlemleri yapılabilir.
    /// </summary>
    [Area("Admin")]
    [Route("Admin/ExchangeRates")]
    public class ExchangeRateController : Controller
    {
        private readonly IExchangeRateManager _exchangeRateManager;
        private readonly IMapper _mapper;

        // 📌 Kur verileriyle ilgili işlemleri yönetecek controller
        public ExchangeRateController(IExchangeRateManager exchangeRateManager, IMapper mapper)
        {
            _exchangeRateManager = exchangeRateManager;
            _mapper = mapper;
        }

        /// <summary>
        /// Tüm döviz kuru kayıtlarını listeler.
        /// </summary>
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            // 💾 Tüm DTO kayıtlarını al
            List<ExchangeRateDto> dtoList = await _exchangeRateManager.GetAllAsync();

            // 🔁 DTO → ViewModel dönüşümü
            List<ExchangeRateResponseModel> vmList = _mapper.Map<List<ExchangeRateResponseModel>>(dtoList);

            return View(vmList);
        }

        /// <summary>
        /// Yeni döviz kuru ekleme formunu getirir.
        /// </summary>
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Yeni kur kaydını işler.
        /// </summary>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(ExchangeRateRequestModel model)
        {
            // 📌 ModelState doğrulaması
            if (!ModelState.IsValid)
                return View(model);

            // 📥 ViewModel → DTO dönüşümü
            ExchangeRateDto dto = _mapper.Map<ExchangeRateDto>(model);

            // 💾 Veritabanına kaydet
            await _exchangeRateManager.CreateAsync(dto);

            TempData["Success"] = "Kur bilgisi başarıyla eklendi.";
            return RedirectToAction("Index");
        }
        /// <summary>
        /// Güncelleme formunu verilerle birlikte getirir.
        /// </summary>
        [HttpGet("Update/{id}")]
        public async Task<IActionResult> Update(int id)
        {
            ExchangeRateDto? dto = await _exchangeRateManager.GetByIdAsync(id);
            if (dto == null)
                return NotFound();

            ExchangeRateRequestModel vm = _mapper.Map<ExchangeRateRequestModel>(dto);
            return View(vm);
        }

        /// <summary>
        /// Güncellenen kur bilgilerini veritabanına kaydeder.
        /// </summary>
        [HttpPost("Update/{id}")]
        public async Task<IActionResult> Update(int id, ExchangeRateRequestModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            ExchangeRateDto dto = _mapper.Map<ExchangeRateDto>(model);
            dto.Id = id;

            await _exchangeRateManager.UpdateAsync(dto);
            TempData["Success"] = "Kur bilgisi başarıyla güncellendi.";
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Seçilen kur kaydını siler.
        /// </summary>
        [HttpPost("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            bool result = await _exchangeRateManager.DeleteAsync(id);

            TempData["Success"] = result
                ? "Kur bilgisi silindi."
                : "Silme işlemi başarısız oldu.";

            return RedirectToAction("Index");
        }
    }
}
