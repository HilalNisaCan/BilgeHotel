

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.Entities.Models;

namespace Project.MvcUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TestController : Controller
    {
        private readonly UserManager<User> _userManager;
   

        public TestController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
           
        }

        public async Task<IActionResult> CreateAdmin()
        {
            string userName = "Duman";
            string email = "duman@bilgehotel.com";
            string password = "duman123";

            User existingUser = await _userManager.FindByNameAsync(userName);
            if (existingUser != null)
                return Content("⚠️ Kullanıcı zaten var!");

            User user = new User
            {
                UserName = userName,
                Email = email,
                EmailConfirmed = true,
                Role = Project.Entities.Enums.UserRole.Receptionist
            };

            IdentityResult result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
                return Content("❌ Hata: " + string.Join(" | ", result.Errors.Select(x => x.Description)));

            await _userManager.AddToRoleAsync(user, "Receptionist");

            return Content("✅ duman başarıyla oluşturuldu! Şifre: " + password);
        }
        public async Task<IActionResult> CreateReceptionist()
        {
            string userName = "hilalnisacan";
            string email = "resepsiyon@bilgehotel.com";
            string password = "duman123";

            User existingUser = await _userManager.FindByNameAsync(userName);
            if (existingUser != null)
                return Content("⚠️ Kullanıcı zaten var!");

            User user = new User
            {
                UserName = userName,
                Email = email,
                EmailConfirmed = true,
                Role = Project.Entities.Enums.UserRole.Receptionist
            };

            IdentityResult result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
                return Content("❌ Hata: " + string.Join(" | ", result.Errors.Select(x => x.Description)));

            await _userManager.AddToRoleAsync(user, "Receptionist");

            return Content("✅ Resepsiyonist başarıyla oluşturuldu! Şifre: " + password);
        }
    }
}
