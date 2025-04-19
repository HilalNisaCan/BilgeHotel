using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Project.BLL.DtoClasses;
using Project.BLL.Managers.Abstracts;
using Project.BLL.Managers.Concretes;
using Project.Entities.Enums;
using Project.MvcUI.Models.PureVm.RequestModel.Contact;

namespace Project.MvcUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IComplaintLogManager _complaintLogManager;
        private readonly IMapper _mapper;

        public HomeController(IMapper mapper,IComplaintLogManager complaintLogManager)
        {
            _mapper = mapper;
            _complaintLogManager = complaintLogManager;
        }
 
         



        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Contact(string fullName, string email, string Subject , string message)
        {
            ComplaintLogDto dto = new ComplaintLogDto
            {
                FullName = fullName,
                Email = email,
                Subject=Subject,
                Description = message,
                SubmittedDate = DateTime.Now,
                Status = ComplaintStatus.Pending,
                Response = "",
                IsResolved = false
            };

            await _complaintLogManager.CreateAsync(dto);

            TempData["Message"] = "Mesajınız başarıyla alındı. En kısa sürede dönüş yapılacaktır.";
            return RedirectToAction("Contact");
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
