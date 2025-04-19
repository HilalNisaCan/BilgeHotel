using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.BLL.DtoClasses;
using Project.BLL.Managers.Abstracts;
using Project.BLL.Managers.Concretes;
using Project.Entities.Enums;
using Project.Entities.Models;
using Project.MvcUI.Models.PureVm.ResponseModel.Review;
using Project.MvcUI.Models.PureVm.ResponseModel.Room;
using Project.MvcUI.Services;
using System.Security.Claims;


namespace Project.MvcUI.Controllers
{
    [Route("Rooms")]
    public class RoomController : Controller
    {
        private readonly IRoomManager _roomManager;
        private readonly IMapper _mapper;
        private readonly RoomTypePriceApiClient _roomTypePriceApiClient;
        private readonly IReviewManager _reviewManager;
        private readonly IReservationManager _reservationManager;

        public RoomController(IRoomManager roomManager, IMapper mapper, RoomTypePriceApiClient roomTypePriceApiClient, IReviewManager reviewManager, IReservationManager reservationManager)
        {
            _roomManager = roomManager;
            _mapper = mapper;
            _roomTypePriceApiClient = roomTypePriceApiClient;
            _reviewManager = reviewManager;
            _reservationManager = reservationManager;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            List<RoomDto> rooms = await _roomManager.GetAllWithImagesAsync();

            List<RoomDto> distinctRooms = rooms
                .GroupBy(r => r.RoomType)
                .Select(g => g.First())
                .ToList();

            List<RoomResponseModel> modelList = _mapper.Map<List<RoomResponseModel>>(distinctRooms);

            foreach (RoomResponseModel room in modelList)
            {
                decimal? apiPrice = await _roomTypePriceApiClient.GetPriceByRoomTypeAsync(room.RoomType);
                if (apiPrice.HasValue)
                    room.PricePerNight = apiPrice.Value;

                // 🟡 Yorum bilgilerini getir
                room.AverageRating = await _reviewManager.GetAverageRatingByRoomTypeAsync(room.RoomType);
                List<ReviewDto> reviews = await _reviewManager.GetReviewsByRoomTypeAsync(room.RoomType);
                room.ReviewCount = reviews.Count(r => r.IsApproved);
            }

            return View(modelList);
        }
        [HttpGet("Room/Details")]
        public async Task<IActionResult> Details(RoomType roomType)
        {
            Room room = await _roomManager.GetFirstOrDefaultAsync(
                predicate: x => x.RoomType == roomType,
                include: x => x.Include(r => r.RoomImages)
            );

            if (room == null)
                return NotFound();

            RoomDto roomDto = _mapper.Map<RoomDto>(room);
            RoomResponseModel model = _mapper.Map<RoomResponseModel>(roomDto);

            var apiPrice = await _roomTypePriceApiClient.GetPriceByRoomTypeAsync(model.RoomType);
            if (apiPrice.HasValue)
                model.PricePerNight = apiPrice.Value;

            model.AverageRating = await _reviewManager.GetAverageRatingByRoomTypeAsync(model.RoomType);
            List<ReviewDto> reviews = await _reviewManager.GetReviewsByRoomTypeAsync(model.RoomType);
            model.ReviewCount = reviews.Count;

            model.reviewDisplays = reviews
                .Where(r => r.IsApproved)
                .OrderByDescending(r => r.CommentDate)
                .Select(r => new ReviewDisplayModel
                {
                    CustomerName = r.IsAnonymous ? "Anonim Kullanıcı" : "Kullanıcı",
                    Comment = r.Comment,
                    Rating = r.Rating,
                    CreatedDate = r.CommentDate
                }).ToList();

            model.CanComment = User.Identity != null && User.Identity.IsAuthenticated;

            return View("Details", model); // aynı View kullanılır
        }

        [HttpPost("Room/AddReview")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddReview(ReviewDto model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Yorum gönderilirken hata oluştu. Lütfen formu kontrol edin.";
                return RedirectToAction("Details", new { roomType = model.RoomType });
            }

            model.CommentDate = DateTime.Now;
            model.IsApproved = false;

            bool result = await _reviewManager.AddReviewAsync(model);

            TempData[result ? "Success" : "Error"] = result
                ? "Yorumunuz alındı. Onaylandıktan sonra yayınlanacaktır."
                : "Yorum gönderilemedi.";

            return RedirectToAction("Details", new { roomType = model.RoomType });
        }
    }
}
