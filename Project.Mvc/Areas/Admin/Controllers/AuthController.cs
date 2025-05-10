using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Entities.Enums;
using Project.Entities.Models;
using Project.MvcUI.Areas.Admin.Models.PureVm.RequestModel.AppUser;

namespace Project.MvcUI.Areas.Admin.Controllers
{
   
    /*“AuthController, yalnızca Admin rolüne sahip kullanıcıların giriş yapabildiği bir yönetici kimlik doğrulama katmanıdır.
Sisteme giriş UserManager ve SignInManager üzerinden sağlanır.
Kullanıcı, e-posta veya kullanıcı adı ile giriş yapabilir, ancak sisteme erişebilmesi için AppRole.Name = Admin olması gerekir.
Giriş doğrulaması, şifre kontrolü ve yetki denetimi başarılı olursa kullanıcı dashboard’a yönlendirilir.
Şifre sıfırlama süreçleri Identity tabanlı token yapısıyla çalışır ve test amaçlı olarak sıfırlama linki doğrudan ekrana basılır.
Ayrıca logout, yetkisiz erişim yönlendirmesi (AccessDenied) ve şifre resetleme işlemleri de bu controller üzerinden yönetilir.
Yapı, ASP.NET Identity mimarisine uyumlu şekilde güvenli, test edilebilir ve genişletilebilir olarak yapılandırılmıştır.”*/


    [Area("Admin")]
    [AllowAnonymous]
    public class AuthController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        
        private readonly UserManager<User> _userManager;

        public AuthController(SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        /// <summary>
        /// Giriş formunu açar. Kullanıcı zaten giriş yaptıysa bilgi mesajı döner.
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
                return Content("Login ekranı açılıyor...");

            return View();
        }

        /// <summary>
        /// Giriş formu post edildiğinde kullanıcıyı kontrol eder, şifre doğruluğunu denetler,
        /// sadece Admin rolüne sahip kullanıcıların girişine izin verir.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Login(AdminLoginRequestModel model)
        {

            if (!ModelState.IsValid)
                return View(model);

            // Kullanıcıyı e-posta veya kullanıcı adına göre bul
            User? foundUser = await _userManager.Users
                .FirstOrDefaultAsync((User u) =>
                    u.Email == model.EmailOrUsername || u.UserName == model.EmailOrUsername);

            if (foundUser == null)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı bulunamadı.");
                return View(model);
            }

            // Kullanıcının Admin rolünde olup olmadığını kontrol et
            bool isAdmin = await _userManager.IsInRoleAsync(foundUser, "Admin");
            if (!isAdmin)
            {
                ModelState.AddModelError(string.Empty, "Yetkiniz bulunmamaktadır.");
                return View(model);
            }

            // Giriş yapmayı dene
            Microsoft.AspNetCore.Identity.SignInResult loginResult =
                await _signInManager.PasswordSignInAsync(foundUser, model.Password, false, true);

            if (loginResult.Succeeded)
                return RedirectToAction("Index", "Dashboard");

            ModelState.AddModelError(string.Empty, "Giriş başarısız. Bilgileri kontrol edin.");
            return View(model);
        }

        /// <summary>
        /// Kullanıcı çıkış işlemini yapar.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        /// <summary>
        /// Yetkisiz erişim durumunda açılan sayfa.
        /// </summary>
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        /// <summary>
        /// Şifremi unuttum formu (GET)
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        /// <summary>
        /// Şifremi unuttum işlemi. E-posta kontrolü ve token oluşturur.
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequestModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            User user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                ModelState.AddModelError("", "Geçerli bir e-posta giriniz.");
                return View(model);
            }

            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            string resetLink = Url.Action("ResetPassword", "Auth", new { area = "Admin", email = user.Email, token = token }, Request.Scheme);

            return Content($"📩 Şifre sıfırlama linki: {resetLink}");
        }



        /// <summary>
        /// Şifre sıfırlama sayfasını açar (token ve e-mail ile birlikte)
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string email, string token)
        {
            return View(new ResetPasswordRequestModel { Email = email, Token = token });
        }

        /// <summary>
        /// Yeni şifre belirleme işlemini gerçekleştirir.
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequestModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            User user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Kullanıcı bulunamadı.");
                return View(model);
            }

            IdentityResult result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);

            if (result.Succeeded)
                return RedirectToAction("Login", "Auth", new { area = "Admin" });

            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }
    }
}
