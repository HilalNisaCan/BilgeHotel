using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Project.BLL.DtoClasses;
using Project.BLL.Managers.Abstracts;
using Project.MvcUI.Areas.Admin.Models.PageVm;
using Project.MvcUI.Areas.Admin.Models.PureVm.RequestModel.RoomTypePrice;

namespace Project.MvcUI.Areas.Admin.Controllers
{
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



        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            RoomTypePriceDto? dto = await _roomTypePriceManager.GetByIdAsync(id);
            if (dto == null)
                return NotFound();

            return View(dto); // View: Edit.cshtml
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, RoomTypePriceDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _roomTypePriceManager.UpdateRoomTypePriceAsync(id, model);
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
