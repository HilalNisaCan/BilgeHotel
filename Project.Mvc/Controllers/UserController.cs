using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Dal.ContextClasses;
using Project.Dal.Repositories.Abstracts;
using Project.Dal.Repositories.Concretes;
using Project.Entities.Enums;
using Project.Entities.Models;
using Project.MvcUI.Models.PageVm.User;

namespace Project.MvcUI.Controllers
{

    //“UserController, giriş yapan kullanıcıların kendi profillerini görüntüleyip düzenleyebildiği,
    //geçmiş ve aktif rezervasyonlarını takip edebildiği kişisel kontrol panelidir.”
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserProfileRepository _userProfileRepo;
        private readonly IReservationRepository _reservationRepo;
        private readonly MyContext _context;
        private readonly IMapper _mapper;

        public UserController(UserManager<User> userManager,
                              IUserProfileRepository userProfileRepo,
                              IReservationRepository reservationRepo,MyContext context,IMapper mapper)
        {
            _userManager = userManager;
            _userProfileRepo = userProfileRepo;
            _reservationRepo = reservationRepo;
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Kullanıcı profil bilgilerini ve rezervasyon geçmişini getirir.
        /// </summary>

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            User currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
                return RedirectToAction("Login", "Account");

            UserProfile profile = await _userProfileRepo.GetByUserIdAsync(currentUser.Id);
            if (profile == null)
            {
                profile = new UserProfile
                {
                    UserId = currentUser.Id,
                    FirstName = "İsimsiz",
                    LastName = "Kullanıcı",
                    PhoneNumber = currentUser.PhoneNumber,
                    BirthDate = DateTime.Now.AddYears(-18)
                };
                await _userProfileRepo.CreateAsync(profile);
            }

            DateTime now = DateTime.Now;

            List<Reservation> allReservations = await _context.Reservations
       .AsNoTracking() // 💥 EF cache'i bypass eder, doğrudan DB'den çeker
       .Where(r => r.UserId == currentUser.Id)
       .Include(r => r.Room)
       .ToListAsync();

            foreach (var r in allReservations)
            {
                Console.WriteLine($"[DEBUG] RezId: {r.Id} | Fiyat: {r.TotalPrice} ₺ | Status: {r.ReservationStatus}");
            }

            // ✅ En son onaylanmış rezervasyonu al (gelecek tarihli dahil)
            List<Reservation> currentReservations = allReservations
      .Where(r =>
          (r.ReservationStatus == ReservationStatus.Waiting || r.ReservationStatus == ReservationStatus.Confirmed) &&
          r.EndDate >= now)
      .OrderBy(r => r.StartDate)
      .ToList();
            // ✅ Geçmiş rezervasyonlar
            List<Reservation> pastReservations = allReservations
      .Where(r =>
          r.EndDate < now ||
          r.ReservationStatus == ReservationStatus.Completed ||
          r.ReservationStatus == ReservationStatus.Cancelled)
      .ToList();

            UserProfilePageVm vm = new UserProfilePageVm
            {
                Profile = profile,
                CurrentReservations = currentReservations,
                PastReservations = pastReservations
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(UserProfilePageVm vm)
        {
            if (vm.Profile == null || vm.Profile.UserId == 0)
            {
                TempData["Message"] = "Profil bilgisi geçersiz.";
                return RedirectToAction("Profile");
            }

            var profile = await _userProfileRepo.GetByUserIdAsync(vm.Profile.UserId);
            if (profile == null)
            {
                TempData["Message"] = "Profil bulunamadı.";
                return RedirectToAction("Profile");
            }

            // Tüm alanları güncelle
            profile.FirstName = vm.Profile.FirstName;
            profile.LastName = vm.Profile.LastName;
            profile.PhoneNumber = vm.Profile.PhoneNumber;
            profile.Address = vm.Profile.Address;
            profile.City = vm.Profile.City;
            profile.Country = vm.Profile.Country;
            profile.Nationality = vm.Profile.Nationality;
            profile.Gender = vm.Profile.Gender;
            profile.IdentityNumber = vm.Profile.IdentityNumber;
            profile.BirthDate = vm.Profile.BirthDate;
            profile.ProfileImagePath = vm.Profile.ProfileImagePath;

            await _userProfileRepo.UpdateAsync(profile);

            TempData["Message"] = "Profil bilgileri başarıyla güncellendi.";
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Kullanıcının kendi aktif rezervasyonunu iptal etmesini sağlar.
        /// Sadece bugünden sonraki onaylanmış rezervasyonlar iptal edilebilir.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelReservation(int id)
        {
            User currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
                return RedirectToAction("Login", "Account");

            Reservation? reservation = await _context.Reservations
                .FirstOrDefaultAsync(r => r.Id == id && r.UserId == currentUser.Id);

            if (reservation == null)
            {
                TempData["Error"] = "Rezervasyon bulunamadı veya size ait değil.";
                return RedirectToAction("Profile");
            }

            if (reservation.StartDate <= DateTime.Today)
            {
                TempData["Error"] = "Başlamış veya geçmiş rezervasyon iptal edilemez.";
                return RedirectToAction("Profile");
            }

            reservation.ReservationStatus = ReservationStatus.Cancelled;
            reservation.ModifiedDate = DateTime.Now;
            await _reservationRepo.UpdateAsync(reservation);

            TempData["Message"] = "Rezervasyon başarıyla iptal edildi.";
            return RedirectToAction("Profile");
        }
    }
}
