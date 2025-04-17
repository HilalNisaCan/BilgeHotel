using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.BLL.Managers.Abstracts;
using System.Text;
using System.Xml.Linq;

namespace Project.MvcUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class XmlReportController : Controller
    {
        private readonly IReservationManager _reservationManager;

        public XmlReportController(IReservationManager reservationManager)
        {
            _reservationManager = reservationManager;
        }


        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GenerateTodayGuestXml()
        {
            var today = DateTime.Today;

            var reservations = await _reservationManager.GetAllWithIncludeAsync(
                 predicate: x => x.StartDate.Date == today,
                 include: x => x.Include(r => r.Customer)
                   .ThenInclude(c => c.User)
                   .ThenInclude(u => u.UserProfile)
            );

            if (!reservations.Any())
            {
                TempData["Error"] = "Bugün giriş yapan müşteri bulunamadı.";
                return RedirectToAction("Index", "XmlReport");
            }

            XElement xml = new XElement("Guests",
            reservations.Select(r => new XElement("Guest",
            new XElement("FirstName", r.Customer.User.UserProfile.FirstName),
            new XElement("LastName", r.Customer.User.UserProfile.LastName),
            new XElement("IdentityNumber", r.Customer.IdentityNumber),
            new XElement("CheckInDate", r.StartDate.ToString("yyyy-MM-dd")),
            new XElement("CheckOutDate", r.EndDate.ToString("yyyy-MM-dd")),
            new XElement("RoomId", r.RoomId)
            ))
            );

            byte[] xmlBytes = Encoding.UTF8.GetBytes(xml.ToString());

            return File(xmlBytes, "application/xml", $"KimlikBildirim_{today:yyyyMMdd}.xml");
        }
    }
}

