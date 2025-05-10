using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Project.BLL.DtoClasses;
using Project.BLL.Managers.Abstracts;
using Project.Entities.Enums;
using Project.MvcUI.Areas.Admin.Models.PureVm.RequestModel.Complaint;
using Project.MvcUI.Areas.Admin.Models.PureVm.ResponseModel.Complaint;

namespace Project.MvcUI.Areas.Admin.Controllers
{
  
    /*“ComplaintLogController, tamamen DTO tabanlı çalışacak şekilde yapılandırılmıştır.
Listeleme, detay görüntüleme ve silme işlemleri ComplaintLogDto üzerinden gerçekleştirilirken,
yanıt işlemi için özel olarak ComplaintReplyRequestModel kullanılmıştır.
Entity ile doğrudan temas yoktur; tüm dönüşümler AutoMapper aracılığıyla yapılır.
Bu yapı hem katmanlı mimariye hem de SOLID prensiplerine birebir uygundur.”*/


    [Area("Admin")]
    public class ComplaintLogController : Controller
    {
        private readonly IComplaintLogManager _complaintLogManager;
        private readonly IMapper _mapper;

        public ComplaintLogController(IComplaintLogManager complaintLogManager,IMapper mapper)
        {
            _complaintLogManager = complaintLogManager;
            _mapper = mapper;
        }

        /// <summary>
        /// Tüm şikayet kayıtlarını listeler (DTO → ResponseModel dönüşümü yapılır)
        /// </summary>
        public async Task<IActionResult> Index()
        {
            List<ComplaintLogDto> dtoList = await _complaintLogManager.GetAllAsync();
            List<ComplaintLogResponseModel> modelList = _mapper.Map<List<ComplaintLogResponseModel>>(dtoList);
            return View(modelList);
        }

        /// <summary>
        /// Belirli bir şikayetin detayını getirir (DTO → ResponseModel)
        /// </summary>
        public async Task<IActionResult> Details(int id)
        {
            ComplaintLogDto dto = await _complaintLogManager.GetByIdAsync(id);
            if (dto == null)
                return NotFound();

            ComplaintLogResponseModel model = _mapper.Map<ComplaintLogResponseModel>(dto);
            return View(model);
        }

        /// <summary>
        /// Şikayet yanıt formunu getirir (GET) (DTO → RequestModel)
        /// </summary>
        public async Task<IActionResult> Reply(int id)
        {
            ComplaintLogDto dto = await _complaintLogManager.GetByIdAsync(id);
            if (dto == null)
                return NotFound();

            ComplaintReplyRequestModel model = _mapper.Map<ComplaintReplyRequestModel>(dto);
            return View(model);
        }


        /// <summary>
        /// Şikayet yanıtını işler (POST) (RequestModel kullanımı)
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Reply(ComplaintReplyRequestModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            bool result = await _complaintLogManager.RespondToComplaintAsync(model.Id, model.Response);
            if (!result)
                return NotFound();

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Şikayet kaydını siler (id parametresi ile)
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            bool result = await _complaintLogManager.DeleteComplaintAsync(id);
            TempData["Message"] = result ? "Şikayet başarıyla silindi." : "Şikayet silinemedi.";
            return RedirectToAction("Index");
        }
    }
}
