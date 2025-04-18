using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.BLL.DtoClasses;
using Project.BLL.Managers.Abstracts;
using Project.Entities.Enums;
using Project.Entities.Models;
using System.Text;
using System.Xml.Linq;
using ReservationEntity = Project.Entities.Models.Reservation;

namespace Project.MvcUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class XmlReportController : Controller
    {
        private readonly IReservationManager _reservationManager;
        private readonly IReportLogManager _reportLogManager;
        private readonly IMapper _mapper;

        public XmlReportController(IReservationManager reservationManager, IReportLogManager reportLogManager,IMapper mapper)
        {
            _reservationManager = reservationManager;
            _reportLogManager = reportLogManager;
            _mapper = mapper;
        }


        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GenerateTodayGuestXml()
        {
            DateTime today = DateTime.Today;

            // 1️⃣ Bugünkü giriş yapan rezervasyonları çekiyoruz
            List<ReservationEntity> reservationEntities = await _reservationManager.GetAllWithIncludeAsync(
                predicate: x => x.StartDate.Date == today,
                include: x => x.Include(r => r.Customer)
                               .ThenInclude(c => c.User)
                               .ThenInclude(u => u.UserProfile)
            );

            List<ReservationDto> reservations = _mapper.Map<List<ReservationDto>>(reservationEntities);

            if (!reservations.Any())
            {
                TempData["Error"] = "Bugün giriş yapan müşteri bulunamadı.";
                return RedirectToAction("Index", "XmlReport");
            }

            // 2️⃣ XML içeriğini oluşturuyoruz
            XElement xml = new XElement("Guests",
                reservations.Select(r => new XElement("Guest",
                    new XElement("FirstName", r.Customer.FirstName),
                   new XElement("LastName", r.Customer.LastName),
                    new XElement("IdentityNumber", r.Customer?.IdentityNumber ?? "Belirsiz"),
                    new XElement("CheckInDate", r.StartDate.ToString("yyyy-MM-dd")),
                    new XElement("CheckOutDate", r.EndDate.ToString("yyyy-MM-dd")),
                    new XElement("RoomId", r.RoomId)
                ))
            );

            // 3️⃣ XML dosyasını kaydet
            string reportsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "XmlReports");
            if (!Directory.Exists(reportsFolder))
                Directory.CreateDirectory(reportsFolder);

            string fileName = $"KimlikBildirim_{today:yyyyMMdd}.xml";
            string filePath = Path.Combine(reportsFolder, fileName);
            await System.IO.File.WriteAllTextAsync(filePath, xml.ToString());
            byte[] xmlBytes = Encoding.UTF8.GetBytes(xml.ToString());

            // Rapor log kaydı
            ReportLogDto dto = new ReportLogDto
            {
                ReportType = ReportType.DailyGuestReport,
                ReportDate = DateTime.Now,
                ReportStatus = ReportStatus.Success,
                LogMessage = "Günlük müşteri girişi XML raporu oluşturuldu.",
                ReportData = xml.ToString(),
                IsSystemGenerated = true,
                XmlFilePath = $"/XmlReports/{fileName}",
                IPAddress = HttpContext.Connection?.RemoteIpAddress?.ToString()
            };

            TempData["Success"] = "Günlük müşteri XML raporu oluşturuldu ve kayıt altına alındı.";
            return File(xmlBytes, "application/xml", fileName);
        }
    }
}

