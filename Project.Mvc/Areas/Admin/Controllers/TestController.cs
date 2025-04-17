

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.Entities.Models;
using System.Security.Claims;

namespace Project.MvcUI.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class TestController : Controller
    {
        [HttpGet("/Admin/LoginAsAdmin")]
        public async Task<IActionResult> LoginAsAdmin()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "hilalnisa.canpolat@bilgeadamakademi.com"),
                new Claim(ClaimTypes.Role, "6"), // Admin
                new Claim("UserId", "31")        // Veritabanındaki User.Id
            };

            var identity = new ClaimsIdentity(claims, IdentityConstants.ApplicationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, principal);

            return RedirectToAction("Index", "Home");
        }
    }
}
//public async Task<IActionResult> CreateReceptionist()
//{
//    string userName = "hilalnisacan";
//    string email = "resepsiyon@bilgehotel.com";
//    string password = "duman123";

//    User existingUser = await _userManager.FindByNameAsync(userName);
//    if (existingUser != null)
//        return Content("⚠️ Kullanıcı zaten var!");

//    User user = new User
//    {
//        UserName = userName,
//        Email = email,
//        EmailConfirmed = true,
//        Role = Project.Entities.Enums.UserRole.Receptionist
//    };

//    IdentityResult result = await _userManager.CreateAsync(user, password);

//    if (!result.Succeeded)
//        return Content("❌ Hata: " + string.Join(" | ", result.Errors.Select(x => x.Description)));

//    await _userManager.AddToRoleAsync(user, "Receptionist");

//    return Content("✅ Resepsiyonist başarıyla oluşturuldu! Şifre: " + password);
//}