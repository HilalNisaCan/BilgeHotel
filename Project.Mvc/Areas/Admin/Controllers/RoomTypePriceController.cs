using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Project.BLL.DtoClasses;
using Project.BLL.Managers.Abstracts;
using Project.MvcUI.Areas.Admin.Models.PageVm;
using Project.MvcUI.Areas.Admin.Models.PureVm.RequestModel.RoomTypePrice;
using Project.MvcUI.Areas.Admin.Models.PureVm.ResponseModel.RoomTypePrice;

namespace Project.MvcUI.Areas.Admin.Controllers
{
    /*"RoomTypePriceController, oda türlerine ait fiyatların listelenmesi, güncellenmesi ve silinmesini sağlayan yönetici panelidir.
Tüm veriler RoomTypePriceDto üzerinden çekilir ve RoomTypePriceResponseModel'e AutoMapper aracılığıyla dönüştürülür.
Güncelleme işlemleri ViewModel → DTO dönüşümüyle yapılır ve TempData üzerinden kullanıcıya işlem sonuçları bildirilir.
Kod yapısı açık tiplerle yazılmıştır, var kullanılmamıştır. Katmanlı mimari ve temiz MVC prensiplerine uygundur."

* "Fiyatlar Enum olan RoomType bilgisine göre eşleştirilir."

* "Fiyatlar sayfası ViewModel (PageVm) aracılığıyla taşınır, DTO'lar direkt View’a gönderilmez."

*"Mapping işlemleri profilde RoomTypePriceDto → ResponseModel şeklinde yapılandırılmıştır." */


    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class RoomTypePriceController : Controller
    {
        private readonly IRoomTypePriceManager _roomTypePriceManager;
        private readonly IMapper _mapper;

        public RoomTypePriceController(IRoomTypePriceManager roomTypePriceManager, IMapper mapper)
        {
            _roomTypePriceManager = roomTypePriceManager;
            _mapper = mapper;
        }

        // 📄 Listeleme
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            RoomTypePricePageVm vm = new RoomTypePricePageVm
            {
                RoomTypePrices = await _roomTypePriceManager.GetAllRoomTypePricesAsync()
            };

            return View(vm);
        }



        // [GET] Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            RoomTypePriceDto? dto = await _roomTypePriceManager.GetByIdAsync(id);
            if (dto == null) return NotFound();

            RoomTypePriceResponseModel model = _mapper.Map<RoomTypePriceResponseModel>(dto);
            return View(model); // View: Edit.cshtml
        }


        // [POST] Edit
        [HttpPost]
        public async Task<IActionResult> Edit(int id, RoomTypePriceResponseModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            RoomTypePriceDto dto = _mapper.Map<RoomTypePriceDto>(model);
            await _roomTypePriceManager.UpdateRoomTypePriceAsync(id, dto);

            TempData["Message"] = "✅ Fiyat başarıyla güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _roomTypePriceManager.DeleteRoomTypePriceAsync(id);
            TempData["Message"] = "🗑️ Fiyat başarıyla silindi.";
            return RedirectToAction(nameof(Index));
        }
    }

}
