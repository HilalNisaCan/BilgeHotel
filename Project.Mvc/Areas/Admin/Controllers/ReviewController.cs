using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Project.BLL.DtoClasses;
using Project.BLL.Managers.Abstracts;
using Project.Entities.Models;
using Project.MvcUI.Areas.Admin.Models.PureVm.ResponseModel.Review;

namespace Project.MvcUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Reviews")]
    public class ReviewController : Controller
    {
        private readonly IReviewManager _reviewManager;
        private readonly IMapper _mapper;

        public ReviewController(IReviewManager reviewManager, IMapper mapper)
        {
            _reviewManager = reviewManager;
            _mapper = mapper;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            List<ReviewDto> pendingReviews = await _reviewManager.GetPendingReviewsAsync();

            List<ReviewAdminResponseModel> model = pendingReviews.Select(r => new ReviewAdminResponseModel
            {
                Id = r.Id,
                UserFullName = r.UserFirstName + " " + r.UserLastName,
                Comment = r.Comment,
                Rating = r.Rating,
                CommentDate = r.CommentDate,
                IsApproved = r.IsApproved,
                RoomType = r.RoomType
            }).ToList();

            return View(model);
        }

        [HttpPost("Approve")]
        public async Task<IActionResult> Approve(int id)
        {
            bool result = await _reviewManager.ApproveReviewAsync(id);
            TempData["Message"] = result ? "Yorum onaylandı ✅" : "Yorum onaylanamadı ❌";
            return RedirectToAction("Index");
        }

        [HttpPost("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            bool result = await _reviewManager.DeleteAsync(id);
            TempData["Message"] = result ? "Yorum silindi 🗑️" : "Silme işlemi başarısız ❌";
            return RedirectToAction("Index");
        }
    }
}
