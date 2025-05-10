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

    /*AccountController, BilgeHotel web kullanıcılarının
     * kayıt, giriş, aktivasyon ve şifre sıfırlama işlemlerini yönetir. 
     * AutoMapper ile ViewModel → DTO → Entity dönüşümleri uygulanmış, 
     * açık tip kullanımı benimsenmiş ve tüm işlemler katmanlı mimariye uygun şekilde yapılandırılmıştır.
     * Kimlik doğrulama Mernis servisi ile yapılır ve e-posta aktivasyonu dahil güvenli kullanıcı kaydı sağlanır.*/


    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IAppRoleRepository _appRoleRepository;
        private readonly IMapper _mapper;
        private readonly MyContext _context;
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IIdentityValidationService _identityValidationService;

        public AccountController(UserManager<User> userManager,
                                 SignInManager<User> signInManager,            
                                 IMapper mapper,
                                 MyContext context,IUserProfileRepository userProfileRepository,IIdentityValidationService ıdentityValidationService,IAppRoleRepository appRoleRepository) // ← bunu da alıyoruz
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _context = context; // ← tanımlamayı da yap
            _userProfileRepository = userProfileRepository;
            _identityValidationService = ıdentityValidationService;
            _appRoleRepository = appRoleRepository;
        }

        /// <summary>
        /// Kayıt formunu görüntüler.
        /// </summary>
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// Yeni kullanıcı kaydı oluşturur ve aktivasyon maili gönderir.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            KimlikBilgisiDto kimlikDto = _mapper.Map<KimlikBilgisiDto>(model);

            bool isValidIdentity = await _identityValidationService.VerifyAsync(kimlikDto);
            if (!isValidIdentity)
            {
                ModelState.AddModelError("", "Kimlik doğrulaması başarısız. Bilgilerinizi kontrol edin.");
                return View(model);
            }

            User user = _mapper.Map<User>(model);
            user.UserName = model.Email;
            user.EmailConfirmed = false;
            user.IsActivated = false;
            user.ActivationCode = Guid.NewGuid();
            user.WantsCampaignEmails = model.WantsCampaignEmails;

            IdentityResult result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                foreach (IdentityError error in result.Errors)
                    ModelState.AddModelError("", error.Description);
                return View(model);
            }
            // ✅ KULLANICIYA CUSTOMER ROLÜNÜ VERİYORUZ VE AppRoleId ALANINI DOLDURUYORUZ
            AppRole? role = await _appRoleRepository.GetByNameAsync("Customer");
            if (role != null)
            {
                user.AppRoleId = role.Id;
            }
            else
            {
                Console.WriteLine("❗ 'Customer' rolü bulunamadı.");
            }
            UserProfile profile = _mapper.Map<UserProfile>(model);
            profile.UserId = user.Id;
            await _userProfileRepository.CreateAsync(profile);

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

            return RedirectToAction("Login");
        }

        /// <summary>
        /// E-posta aktivasyon bağlantısını doğrular.
        /// </summary>
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
            }
            else
            {
                TempData["Message"] = "Geçersiz aktivasyon bağlantısı.";
            }

            return RedirectToAction("Login");
        }


        /// <summary>
        /// Giriş formunu gösterir.
        /// </summary>
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        /// <summary>
        /// Kullanıcı giriş işlemini gerçekleştirir.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            User user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                TempData["Message"] = "Kullanıcı bulunamadı.";
                return View(model);
            }

            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(user.Email, model.Password, model.RememberMe, false);

            TempData["Message"] = result.Succeeded
                ? "Giriş başarılı."
                : "Giriş başarısız. Şifre veya e-posta hatalı.";

            return result.Succeeded
                ? RedirectToAction("Index", "Home")
                : View(model);
        }

        /// <summary>
        /// Şifremi unuttum formu (GET)
        /// </summary>
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        /// <summary>
        /// Şifremi unuttum işlemini başlatır ve mail gönderir.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            User user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                TempData["Message"] = "Eğer sistemde kayıtlı bir hesabınız varsa e-posta gönderilmiştir.";
                return RedirectToAction("Login");
            }

            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            string link = Url.Action("ResetPassword", "Account", new { token, id = user.Id }, Request.Scheme);

            string body = $"Şifrenizi yenilemek için tıklayın: <a href='{link}'>Şifre Sıfırla</a>";
            EmailService.Send(user.Email, body: body, subject: "BilgeHotel - Şifre Sıfırlama");

            TempData["Message"] = "Eğer e-posta adresiniz sistemde kayıtlıysa, sıfırlama bağlantısı gönderilmiştir.";
            return RedirectToAction("Login");
        }


        /// <summary>
        /// Şifre sıfırlama sayfasını getirir.
        /// </summary>
        [HttpGet]
        public IActionResult ResetPassword(string token, string id)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(id))
                return RedirectToAction("Login");

            ResetPasswordViewModel model = new ResetPasswordViewModel { Token = token, UserId = id };
            return View(model);
        }

        /// <summary>
        /// Yeni şifreyi kaydeder.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            User user = await _userManager.FindByIdAsync(model.UserId);
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

            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(model);
        }

        /// <summary>
        /// Oturumu kapatır ve ana sayfaya yönlendirir.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }

}
