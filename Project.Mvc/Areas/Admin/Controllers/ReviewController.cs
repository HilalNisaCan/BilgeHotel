using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Project.BLL.DtoClasses;
using Project.BLL.Managers.Abstracts;
using Project.Entities.Models;
using Project.MvcUI.Areas.Admin.Models.PureVm.ResponseModel.Review;

namespace Project.MvcUI.Areas.Admin.Controllers
{

    /*"ReviewController, onay bekleyen yorumları yöneticinin incelemesi ve işlemesi için kullanılan modüldür.
Yorumlar DTO olarak çekilir ve AutoMapper aracılığıyla ReviewAdminResponseModel’e dönüştürülür.
Yorumların onaylanması veya silinmesi doğrudan manager üzerinden yapılır.
Kod yapısı sade, okunabilir ve mimariye uygun olacak şekilde açık tiplerle geliştirilmiştir."*/


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

        /// <summary>
        /// Onay bekleyen yorumları listeler.
        /// DTO → ViewModel dönüşümünde AutoMapper kullanılır.
        /// </summary>
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            List<ReviewDto> pendingReviews = await _reviewManager.GetPendingReviewsAsync();
            List<ReviewAdminResponseModel> model = _mapper.Map<List<ReviewAdminResponseModel>>(pendingReviews);
            return View(model);
        }

        /// <summary>
        /// Belirli bir yorumu onaylar.
        /// </summary>
        [HttpPost("Approve")]
        public async Task<IActionResult> Approve(int id)
        {
            bool result = await _reviewManager.ApproveReviewAsync(id);
            TempData["Message"] = result ? "Yorum onaylandı ✅" : "Yorum onaylanamadı ❌";
            return RedirectToAction("Index");
        }


        /// <summary>
        /// Belirli bir yorumu siler.
        /// </summary>
        [HttpPost("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            bool result = await _reviewManager.DeleteAsync(id);
            TempData["Message"] = result ? "Yorum silindi 🗑️" : "Silme işlemi başarısız ❌";
            return RedirectToAction("Index");
        }
    }
}
