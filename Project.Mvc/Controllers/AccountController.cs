using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.BLL.DtoClasses;
using Project.Common.Tools;
using Project.Dal.ContextClasses;
using Project.Dal.Repositories.Abstracts;
using Project.Entities.Enums;
using Project.Entities.Models;
using Project.MvcUI.Models.PureVm.RequestModel.User;
using Project.MvcUI.Models.PureVm.ResponseModel.User;
using Project.BLL.Services;

using Project.BLL.Services.abstracts;


namespace Project.MvcUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;    
        private readonly IMapper _mapper;
        private readonly MyContext _context;
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IIdentityValidationService _identityValidationService;

        public AccountController(UserManager<User> userManager,
                                 SignInManager<User> signInManager,            
                                 IMapper mapper,
                                 MyContext context,IUserProfileRepository userProfileRepository,IIdentityValidationService ıdentityValidationService) // ← bunu da alıyoruz
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _context = context; // ← tanımlamayı da yap
            _userProfileRepository = userProfileRepository;
            _identityValidationService = ıdentityValidationService;
        }
        // 🔹 GET: /Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {

            // 1️⃣ Form verileri geçerli mi?
            if (!ModelState.IsValid)
                return View(model);

            // 2️⃣ TCKimlik doğrulaması (sadece burada yapılır, tekrar yapılmaz)
            KimlikBilgisiDto kimlikDto = new KimlikBilgisiDto
            {
                IdentityNumber = model.IdentityNumber,
                FirstName = model.FirstName,
                LastName = model.LastName,
                BirthYear = model.BirthDate.Year
            };

            bool isValidIdentity = await _identityValidationService.VerifyAsync(kimlikDto);

            if (!isValidIdentity)
            {
                ModelState.AddModelError("", "Kimlik doğrulaması başarısız. Bilgilerinizi kontrol edin.");
                return View(model);
            }

            // 3️⃣ Kullanıcı (User) oluştur
            User user = _mapper.Map<User>(model);
            user.UserName = model.Email;
            user.EmailConfirmed = false;
            user.IsActivated = false;
            user.ActivationCode = Guid.NewGuid();

            IdentityResult result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);
                return View(model);
            }

            // 4️⃣ Profil oluştur
            UserProfile profile = _mapper.Map<UserProfile>(model);
            profile.UserId = user.Id;
            await _userProfileRepository.CreateAsync(profile);

            // 5️⃣ Aktivasyon e-postası gönder
            string link = Url.Action("ConfirmEmail", "Account", new { code = user.ActivationCode, id = user.Id }, Request.Scheme);
            string body = $"""
            Merhaba {model.FirstName},<br/><br/>
            BilgeHotel hesabınızı aktifleştirmek için lütfen aşağıdaki bağlantıya tıklayın:<br/>
            <a href="{link}">Hesabımı Aktifleştir</a><br/><br/>
               Teşekkürler.
            """;

            bool mailSent = EmailService.Send(user.Email, body: body, subject: "BilgeHotel Hesap Aktivasyonu");

            TempData["Message"] = mailSent
                ? "Kayıt başarılı! Aktivasyon e-postası gönderildi."
                : "Kayıt başarılı ancak aktivasyon e-postası gönderilemedi.";

            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(Guid code, string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                TempData["Message"] = "Kullanıcı bulunamadı.";
                return RedirectToAction("Login");
            }

            if (user.ActivationCode == code)
            {
                user.IsActivated = true;
                user.EmailConfirmed = true;
                user.ActivationCode = null;
                await _userManager.UpdateAsync(user);

                TempData["Message"] = "Hesabınız başarıyla aktifleştirildi. Giriş yapabilirsiniz.";
                return RedirectToAction("Login");
            }

            TempData["Message"] = "Geçersiz aktivasyon bağlantısı.";
            return RedirectToAction("Login");
        }
        // 🔹 GET: /Account/Login
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {

            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                TempData["Message"] = "Kullanıcı bulunamadı.";
                return View(model);
            }

            // ✅ Email değil, UserName kullanılmalı
            var result = await _signInManager.PasswordSignInAsync(user.Email, model.Password, model.RememberMe, false);

            if (result.Succeeded)
            {
                TempData["Message"] = "Giriş başarılı.";
                return RedirectToAction("Index", "Home");
            }

            TempData["Message"] = "Giriş başarısız. Şifre veya e-posta hatalı.";
            return View(model);
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {// 1️⃣ Form validasyonu kontrolü
            if (!ModelState.IsValid)
                return View(model);

            // 2️⃣ Email sistemde kayıtlı mı ve onaylı mı?
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                // Güvenlik açısından kullanıcıya detay verilmez
                TempData["Message"] = "Eğer sistemde kayıtlı bir hesabınız varsa e-posta gönderilmiştir.";
                return RedirectToAction("Login");
            }

            // 3️⃣ Token oluştur (şifre sıfırlama için)
            string token = await _userManager.GeneratePasswordResetTokenAsync(user);

            // 4️⃣ Şifre sıfırlama linki oluştur
            string link = Url.Action("ResetPassword", "Account", new
            {
                token,
                id = user.Id
            }, Request.Scheme);

            // 5️⃣ E-posta gönderimi
            string body = $"Şifrenizi yenilemek için tıklayın: <a href='{link}'>Şifre Sıfırla</a>";
            EmailService.Send(user.Email, body: body, subject: "BilgeHotel - Şifre Sıfırlama");

            // 6️⃣ Kullanıcıya mesaj göster ve yönlendir
            TempData["Message"] = "Eğer e-posta adresiniz sistemde kayıtlıysa, sıfırlama bağlantısı gönderilmiştir.";
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult ResetPassword(string token, string id)
        {
            if (token == null || id == null)
                return RedirectToAction("Login");

            var model = new ResetPasswordViewModel { Token = token, UserId = id };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                TempData["Message"] = "Kullanıcı bulunamadı.";
                return RedirectToAction("Login");
            }

            IdentityResult result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);

            if (result.Succeeded)
            {
                TempData["Message"] = "Şifreniz başarıyla güncellendi.";
                return RedirectToAction("Login");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(model);
     
        }


        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }

}
