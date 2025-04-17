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
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserProfileRepository _userProfileRepo;
        private readonly IReservationRepository _reservationRepo;
        private readonly MyContext _context;

        public UserController(UserManager<User> userManager,
                              IUserProfileRepository userProfileRepo,
                              IReservationRepository reservationRepo,MyContext context)
        {
            _userManager = userManager;
            _userProfileRepo = userProfileRepo;
            _reservationRepo = reservationRepo;
            _context = context;
        }

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
                .Where(r => r.UserId == currentUser.Id)
                .Include(r => r.Room)
                .ToListAsync();

            Reservation? currentReservation = allReservations
                .FirstOrDefault(r =>
                    r.StartDate <= now &&
                    r.EndDate >= now &&
                    r.ReservationStatus == ReservationStatus.Confirmed); // ✅ Buraya dikkat!

            List<Reservation> pastReservations = allReservations
                .Where(r => r.EndDate < now || r.ReservationStatus == ReservationStatus.Completed)
                .ToList();

            UserProfilePageVm vm = new UserProfilePageVm
            {
                Profile = profile,
                CurrentReservation = currentReservation,
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
    }
}
