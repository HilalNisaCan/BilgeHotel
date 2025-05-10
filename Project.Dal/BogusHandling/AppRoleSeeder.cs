using Microsoft.AspNetCore.Identity;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Dal.BogusHandling
{
    public static class AppRoleSeeder
    {
        public static async Task SeedAsync(RoleManager<AppRole> roleManager)
        {
            string[] roles = { "Admin", "HR", "Staff", "ReceptionChief", "Receptionist", "IT", "Customer" };

            foreach (var roleName in roles)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    AppRole role = new AppRole
                    {
                        Name = roleName,
                        Description = $"{roleName} rolü - varsayılan açıklama",
                        CreatedDate = DateTime.Now
                    };

                    await roleManager.CreateAsync(role);
                }
            }
        }
    }
}
