using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Project.BLL.DtoClasses;
using Project.BLL.Managers.Abstracts;
using Project.Entities.Enums;
using Project.MvcUI.Areas.Admin.Models.PureVm.RequestModel.Complaint;
using Project.MvcUI.Areas.Admin.Models.PureVm.ResponseModel.Complaint;

namespace Project.MvcUI.Areas.Admin.Controllers
{
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

        // 📄 Şikayetleri listele
        public async Task<IActionResult> Index()
        {
            List<ComplaintLogDto> dtoList = await _complaintLogManager.GetAllAsync();
            List<ComplaintLogResponseModel> modelList = _mapper.Map<List<ComplaintLogResponseModel>>(dtoList);
            return View(modelList);
        }

        // 👁️ Belirli bir şikayetin detayını göster
        public async Task<IActionResult> Details(int id)
        {
            ComplaintLogDto dto = await _complaintLogManager.GetByIdAsync(id);
            if (dto == null)
                return NotFound();

            ComplaintLogResponseModel model = _mapper.Map<ComplaintLogResponseModel>(dto);
            return View(model);
        }

        // ✍️ Yanıtla (GET) – Şikayet formunu getir
        public async Task<IActionResult> Reply(int id)
        {
            // DTO'yu getir
            ComplaintLogDto dto = await _complaintLogManager.GetByIdAsync(id);
            if (dto == null)
                return NotFound();

            // RequestModel'e map'le
            ComplaintReplyRequestModel model = _mapper.Map<ComplaintReplyRequestModel>(dto);

            return View(model);
        }

        // 💾 Yanıtla (POST) – Yanıt kaydını işleme al
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
    }
}
