using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Project.BLL.DtoClasses;
using Project.BLL.Managers.Abstracts;
using Project.Entities.Enums;
using Project.MvcUI.Areas.Admin.Models.PureVm.ResponseModel.Reservation;

namespace Project.MvcUI.Areas.Admin.Controllers
{
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

        // GET: Admin/Reservations
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            List<ReservationDto> dtoList = await _reservationManager.GetAllWithRoomAndCustomerAsync();
            List<ReservationAdminResponseModel> vmList = _mapper.Map<List<ReservationAdminResponseModel>>(dtoList);

            return View(vmList);
        }

        // GET: Admin/Reservations/Approve/5
        [HttpPost("Approve/{id}")]
        public async Task<IActionResult> Approve(int id)
        {
            var reservationDto = await _reservationManager.GetByIdWithRoomAndCustomerAsync(id);
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
        // GET: Admin/Reservations/Cancel/5
        [HttpPost("Cancel/{id}")]
        public async Task<IActionResult> Cancel(int id)
        {
            bool result = await _reservationManager.UpdateStatusAsync(id, ReservationStatus.Cancelled);
            TempData["Message"] = result ? "Rezervasyon iptal edildi 🛑" : "İptal işlemi başarısız ❌";
            TempData["MessageType"] = result ? "warning" : "danger";
            return RedirectToAction("Index");
        }

        // GET: Admin/Reservations/Details/5
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            ReservationDto dto = await _reservationManager.GetByIdWithRoomAndCustomerAsync(id);
            if (dto == null)
                return NotFound();

            ReservationAdminResponseModel vm = _mapper.Map<ReservationAdminResponseModel>(dto);
            return View(vm);
        }
    }
}
