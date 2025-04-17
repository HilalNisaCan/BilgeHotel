using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Project.Entities.Models;
using Project.MvcUI.Areas.Admin.Models.PureVm.RequestModel.Campaign;
using Project.MvcUI.Areas.Admin.Models.PureVm.ResponseModel.Campaign;
using Project.BLL.Managers;
using Project.BLL.Managers.Abstracts;
using Project.BLL.DtoClasses;


namespace Project.MvcUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class CampaignController : Controller
    {
        private readonly ICampainManager _campaignManager;
        private readonly IMapper _mapper;

        public CampaignController(ICampainManager campaignManager, IMapper mapper)
        {
            _campaignManager = campaignManager;
            _mapper = mapper;
        }

        // Listeleme
        [HttpGet]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<CampaignDto> dtoList = await _campaignManager.GetAllAsync();
            List<CampaignResponseModel> modelList = _mapper.Map<List<CampaignResponseModel>>(dtoList);
            return View(modelList);
        }

        // Ekleme GET
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CampaignRequestModel model)
        {
            if (!ModelState.IsValid) return View(model);

            CampaignDto dto = _mapper.Map<CampaignDto>(model);
            await _campaignManager.CreateAsync(dto);

            return RedirectToAction("Index");
        }

        // Güncelleme GET
        [HttpGet("{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            CampaignDto dto = await _campaignManager.GetByIdAsync(id);
            if (dto == null) return NotFound();

            CampaignRequestModel model = _mapper.Map<CampaignRequestModel>(dto);
            return View(model);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> Edit(int id, CampaignRequestModel model)
        {
            if (!ModelState.IsValid) return View(model);

            CampaignDto dto = _mapper.Map<CampaignDto>(model);
            dto.Id = id;

            await _campaignManager.UpdateAsync(dto);
            return RedirectToAction("Index");
        }

        // Silme GET
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var dto = await _campaignManager.GetByIdAsync(id);
            if (dto == null) return NotFound();

            await _campaignManager.DeleteAsync(dto);
            return RedirectToAction("Index");
        }


    }
}
