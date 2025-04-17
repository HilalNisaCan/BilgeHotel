using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.BLL.Managers.Abstracts;

namespace Project.MvcUI.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class DashboardController : Controller
    {
        private readonly ICustomerManager _customerManager;
        private readonly IRoomManager _roomManager;
        private readonly IEmployeeManager _employeeManager;
        private readonly IReservationManager _reservationManager;

        public DashboardController(
            ICustomerManager customerManager,
            IRoomManager roomManager,
            IEmployeeManager employeeManager,
            IReservationManager reservationManager)
        {
            _customerManager = customerManager;
            _roomManager = roomManager;
            _employeeManager = employeeManager;
            _reservationManager = reservationManager;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.TotalCustomers = (await _customerManager.GetAllAsync()).Count;
            ViewBag.TotalRooms = (await _roomManager.GetAllAsync()).Count;
            ViewBag.TotalEmployees = (await _employeeManager.GetAllAsync()).Count;

            // ✅ Bugünkü rezervasyon sayısı için predicate filtreli çekiyoruz
            var today = DateTime.Today;

            var todayReservations = await _reservationManager.GetAllWithIncludeAsync(
                r => r.StartDate.Date == today,
                include: q => q // include kullanmıyorsan q => q bırakabilirsin
            );

            ViewBag.TodayReservations = todayReservations.Count();

            return View();
        }
    }
}
