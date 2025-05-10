using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Project.BLL.DtoClasses;
using Project.BLL.Managers.Abstracts;
using Project.BLL.Managers.Concretes;
using Project.Entities.Enums;
using Project.Entities.Models;
using Project.MvcUI.Models.PureVm.RequestModel.Contact;

namespace Project.MvcUI.Controllers
{
     /// <summary>
    /// 🌐 HomeController 
   /// “HomeController, ziyaretçilerin anasayfa, hakkımızda ve iletişim gibi temel sayfalara erişimini sağlar.
  /// İletişim formu üzerinden gelen mesajlar, ComplaintLog yapısına dönüştürülerek şikayet kaydı olarak sistemde tutulur.
 ///Böylece kullanıcı geri bildirimleri kurumsal hafızada yerini alır.”
/// Web sitesinin genel sayfalarını (Anasayfa, Hakkımızda, İletişim) yöneten controllerdır.
/// /// Kullanıcıların iletişim formundan yaptığı başvurular, şikayet olarak sisteme kayıt edilir.
    /// </summary>
    public class HomeController : Controller
    {
        private readonly IComplaintLogManager _complaintLogManager;
        private readonly IMapper _mapper;

        public HomeController(IMapper mapper,IComplaintLogManager complaintLogManager)
        {
            _mapper = mapper;
            _complaintLogManager = complaintLogManager;
        }


        /// <summary>
        /// Anasayfa
        /// </summary>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Hakkımızda sayfası
        /// </summary>
        public IActionResult About()
        {
            return View();
        }

        /// <summary>
        /// İletişim sayfası (GET)
        /// </summary>
        public IActionResult Contact()
        {
            return View();
        }

        /// <summary>
        /// İletişim formu gönderildiğinde şikayet olarak kayıt oluşturur.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Contact(ContactFormRequestModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // ViewModel → DTO dönüşümü
            ComplaintLogDto dto = _mapper.Map<ComplaintLogDto>(model);
            await _complaintLogManager.CreateAsync(dto);

            TempData["Message"] = "Mesajınız başarıyla alındı. En kısa sürede dönüş yapılacaktır.";
            return RedirectToAction("Contact");
        }

        /// <summary>
        /// Hata sayfası
        /// </summary>
        public IActionResult Error()
        {
            return View();
        }
    }
}
