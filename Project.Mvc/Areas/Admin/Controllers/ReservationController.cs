using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Project.BLL.DtoClasses;
using Project.BLL.Managers.Abstracts;
using Project.Entities.Enums;
using Project.MvcUI.Areas.Admin.Models.PureVm.ResponseModel.Reservation;

namespace Project.MvcUI.Areas.Admin.Controllers
{

    /*Rezervasyonlar hem oda hem müşteri bilgileriyle birlikte DTO olarak çekilir ve AutoMapper ile AdminResponseModel’e dönüştürülür.
Yöneticiler bu panel üzerinden rezervasyonları görüntüleyebilir, detaylarını inceleyebilir, onaylama veya iptal işlemleri gerçekleştirebilir.
Rezervasyon durumu Enum yapısıyla kontrol edilir, böylece sistematik geçişler ve durum takibi yapılabilir.
Tüm işlemler mimariye uygun şekilde DTO-ViewModel ayrımıyla ve  açık tiplerle geliştirilmiştir."*/

    [Area("Admin")]
    [Route("Admin/Reservations")]
    public class ReservationController : Controller
    {
        private readonly IReservationManager _reservationManager;
        private readonly IMapper _mapper;

        public ReservationController(IReservationManager reservationManager, IMapper mapper)
        {
            _reservationManager = reservationManager;
            _mapper = mapper;
        }

        /// <summary>
        /// Tüm rezervasyonları oda ve müşteri bilgileriyle birlikte listeler.
        /// DTO → ViewModel dönüşümü yapılır.
        /// </summary>
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            List<ReservationDto> dtoList = await _reservationManager.GetAllWithRoomAndCustomerAsync();
            List<ReservationAdminResponseModel> vmList = _mapper.Map<List<ReservationAdminResponseModel>>(dtoList);

            return View(vmList);
        }

        /// <summary>
        /// Belirli bir rezervasyonu onaylar.
        /// Eğer zaten onaylanmışsa kullanıcı bilgilendirilir.
        /// </summary>
        [HttpPost("Approve/{id}")]
        public async Task<IActionResult> Approve(int id)
        {
            ReservationDto reservationDto = await _reservationManager.GetByIdWithRoomAndCustomerAsync(id);
            if (reservationDto == null)
                return NotFound();

            if (reservationDto.ReservationStatus == ReservationStatus.Confirmed)
            {
                TempData["Message"] = "Bu rezervasyon zaten onaylanmış.";
                TempData["MessageType"] = "info";
                return RedirectToAction("Index");
            }

            bool result = await _reservationManager.UpdateStatusAsync(id, ReservationStatus.Confirmed);

            TempData["Message"] = result ? "Rezervasyon başarıyla onaylandı ✅" : "Onaylama işlemi başarısız ❌";
            TempData["MessageType"] = result ? "success" : "danger";
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Belirli bir rezervasyonu iptal eder.
        /// </summary>
        [HttpPost("Cancel/{id}")]
        public async Task<IActionResult> Cancel(int id)
        {
            bool result = await _reservationManager.UpdateStatusAsync(id, ReservationStatus.Cancelled);

            TempData["Message"] = result ? "Rezervasyon iptal edildi 🛑" : "İptal işlemi başarısız ❌";
            TempData["MessageType"] = result ? "warning" : "danger";
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Belirli bir rezervasyonun detaylarını gösterir.
        /// DTO → ViewModel dönüşümü yapılır.
        /// </summary>
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            ReservationDto dto = await _reservationManager.GetByIdWithRoomAndCustomerAsync(id);
            if (dto == null)
                return NotFound();

            ReservationAdminResponseModel vm = _mapper.Map<ReservationAdminResponseModel>(dto);
            return View(vm);
        }
        /// <summary>
        /// Belirli bir rezervasyonu kalıcı olarak siler (admin yetkisiyle).
        /// </summary>
        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            bool result = await _reservationManager.DeleteAsync(id);

            TempData["Message"] = result ? "Rezervasyon silindi 🗑️" : "Silme işlemi başarısız ❌";
            TempData["MessageType"] = result ? "danger" : "warning";
            return RedirectToAction("Index");
        }
    }
}
