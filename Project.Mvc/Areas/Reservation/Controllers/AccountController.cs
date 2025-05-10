using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.Common.Tools;
using Project.Entities.Models;
using Project.MvcUI.Areas.Reservation.Models.PureVm.RequestModel.Login;
using Project.MvcUI.Areas.Reservation.Models.PureVm.RequestModel.Register;
using Project.Common;

namespace Project.MvcUI.Areas.Reservation.Controllers
{
    /*"Bu controller, rezervasyon alanındaki kullanıcıların giriş yapma, şifre sıfırlama ve hesap yönetimi işlemlerini yönetiyor. 
     * Tüm işlemler Identity sistemi ile güvenli şekilde gerçekleştirilirken, açık veri tipleriyle sade ve anlaşılır bir yapı kurulmuştur.
     * Kodda katmanlar arası görev ayrımı net bir şekilde yapılmış, böylece bakım ve genişletilebilirlik kolaylaştırılmıştır."*/


    [Area("Reservation")]
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        /// Giriş formunu getirir.
        /// </summary>
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// Giriş formu gönderildiğinde kullanıcıyı doğrular.
        /// Başarılıysa Dashboard'a yönlendirir.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, false, false);

            if (result.Succeeded)
                return RedirectToAction("Index", "Dashboard", new { area = "Reservation" });

            ModelState.AddModelError("", "Geçersiz kullanıcı adı veya şifre.");
            return View(model);
        }

        /// <summary>
        /// Şifremi unuttum formunu getirir.
        /// </summary>
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        /// <summary>
        /// Şifremi unuttum formu gönderildiğinde kullanıcıya şifre sıfırlama linki üretir.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ReservationForgotPasswordViewModel model)
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
            string resetLink = Url.Action("ResetPassword", "Account", new { area = "Reservation", email = user.Email, token = token }, Request.Scheme);

            // Not: Gerçek sistemde mail gönderilecek. Demo’da sadece link gösteriliyor.
            return Content($"📩 Şifre sıfırlama linki: {resetLink}");
        }



        /// <summary>
        /// Şifre sıfırlama formunu getirir.
        /// </summary>
        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            return View(new ResetPasswordViewModel { Email = email, Token = token });
        }

        /// <summary>
        /// Yeni şifreyi sisteme kaydeder.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
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
                return RedirectToAction("Login", "Account", new { area = "Reservation" });

            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }
    }
}
