using Bogus;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project.Dal.ContextClasses;
using Project.Entities.Enums;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Dal.BogusHandling;


/// <summary>
/// Sisteme giriş yapacak kullanıcıları (yöneticiler, resepsiyonistler ve müşteriler) oluşturur.
/// Rolleri tanımlar, kullanıcıları ilgili rollere atar ve AppRoleId eşlemesini yapar.
/// </summary>
public class AppUserSeeder
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly MyContext _context;

    public AppUserSeeder(UserManager<User> userManager, RoleManager<AppRole> roleManager, MyContext context)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _context = context;
    }

    public async Task SeedAsync(List<Employee> employees)
    {
        bool adminExists = _context.Users.Any(u => u.UserName == "salkomut");
        bool receptionistExists = _context.Users.Any(u => u.UserName.Contains("receptionist"));
        bool customerExists = _context.Users.Any(u => u.UserName.Contains("customer"));

        // 🔹 Rolleri oluştur
        string[] roles = { "Admin", "HR", "Staff", "ReceptionChief", "IT", "Receptionist", "Customer" };
        foreach (string role in roles)
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new AppRole
                {
                    Name = role,
                    Description = $"{role} rolü",
                    CreatedDate = DateTime.Now
                });
            }
        }


        Faker faker = new Faker("en");


        // 👑 Yöneticiler
        if (!adminExists)
        {
            var adminList = new List<(string FullName, string Username, string Email, string Role)>
            {
                ("Selahattin Alkomut", "salkomut", "salkomut@bilgehotel.com", "HR"),
                ("Levent Sişarpsoy", "lsi_satis", "levent@bilgehotel.com", "Staff"),
                ("Gülay Aydınlık", "gaydinlik", "gulay@bilgehotel.com", "ReceptionChief"),
                ("Selahattin Karadibag", "skaradibag", "it@bilgehotel.com", "IT"),
                ("Hilal Nisa Can", "hilalnisacan", "hilal@bilgehotel.com", "Admin")
            };

            for (int i = 0; i < adminList.Count; i++)
            {
                var admin = adminList[i];
                // ⚠️ Eğer kullanıcı zaten varsa, geç
                if (_context.Users.Any(u => u.UserName == admin.Username))
                    continue;
                User user = new User
                {
                    UserName = admin.Username,
                    Email = admin.Email,
                    IsActivated = true,
                    ActivationCode = Guid.NewGuid(),
                    CreatedDate = DateTime.Now,
                    Status = DataStatus.Inserted
                };

                IdentityResult result = await _userManager.CreateAsync(user, "123Pa$$word!");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, admin.Role);

                    AppRole? role = await _roleManager.FindByNameAsync(admin.Role);
                    if (role != null)
                    {
                        user.AppRoleId = role.Id;
                        await _userManager.UpdateAsync(user);
                    }

                    if (i < employees.Count)
                    {
                        employees[i].UserId = user.Id;

                        _context.UserProfiles.Add(new UserProfile
                        {
                            UserId = user.Id,
                            FirstName = employees[i].FirstName,
                            LastName = employees[i].LastName,
                            PhoneNumber = employees[i].PhoneNumber,
                            IdentityNumber = faker.Random.Replace("###########"),
                            BirthDate = new DateTime(1990, 1, 1),
                            Address = "Merkez Mahallesi",
                            City = "Istanbul",
                            Country = "Turkey",
                            Nationality = "T.C.",
                            CreatedDate = DateTime.Now,
                            Status = DataStatus.Inserted
                        });
                    }

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"✔️ [YÖNETİCİ EKLENDİ] → {admin.FullName} | Rol: {admin.Role}");
                    Console.ResetColor();
                }
            }
        }

        // 🛎️ Resepsiyonistler
        if (!receptionistExists)
        {
            var receptionists = employees.Where(e => e.Position == EmployeePosition.Receptionist).ToList();

            foreach (var receptionist in receptionists)
            {
                string username = $"receptionist_{receptionist.FirstName.ToLower()}";
                string email = faker.Internet.Email(receptionist.FirstName, receptionist.LastName);

                User user = new User
                {
                    UserName = username,
                    Email = email,
                    IsActivated = true,
                    ActivationCode = Guid.NewGuid(),
                    CreatedDate = DateTime.Now,
                    Status = DataStatus.Inserted
                };

                IdentityResult result = await _userManager.CreateAsync(user, "123Pa$$word!");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Receptionist");

                    AppRole? role = await _roleManager.FindByNameAsync("Receptionist");
                    if (role != null)
                    {
                        user.AppRoleId = role.Id;
                        await _userManager.UpdateAsync(user);
                    }

                    receptionist.UserId = user.Id;

                    _context.UserProfiles.Add(new UserProfile
                    {
                        UserId = user.Id,
                        FirstName = receptionist.FirstName,
                        LastName = receptionist.LastName,
                        PhoneNumber = receptionist.PhoneNumber,
                        IdentityNumber = faker.Random.Replace("###########"),
                        BirthDate = new DateTime(1995, 5, 15),
                        Address = "Hotel Reception",
                        City = "Istanbul",
                        Country = "Turkey",
                        Nationality = "T.C.",
                        CreatedDate = DateTime.Now,
                        Status = DataStatus.Inserted
                    });

                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"👩‍💼 [RESEPSİYONİST EKLENDİ] → {receptionist.FirstName} {receptionist.LastName}");
                    Console.ResetColor();
                }
            }
        }
        // ✅ ÖZEL RESEPSİYONİST → receptionist@bilgehotel.com
        if (!await _userManager.Users.AnyAsync(u => u.UserName == "receptionist"))
        {
            User receptionistUser = new User
            {
                UserName = "receptionist",
                Email = "receptionist@bilgehotel.com",
                IsActivated = true,
                ActivationCode = Guid.NewGuid(),
                CreatedDate = DateTime.Now,
                Status = DataStatus.Inserted
            };

            IdentityResult result = await _userManager.CreateAsync(receptionistUser, "123Pa$$word!");
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(receptionistUser, "Receptionist");

                AppRole? role = await _roleManager.FindByNameAsync("Receptionist");
                if (role != null)
                {
                    receptionistUser.AppRoleId = role.Id;
                    await _userManager.UpdateAsync(receptionistUser);
                }

                _context.UserProfiles.Add(new UserProfile
                {
                    UserId = receptionistUser.Id,
                    FirstName = "Resepsiyon",
                    LastName = "Görevlisi",
                    PhoneNumber = "05001112233",
                    IdentityNumber = "11111111111",
                    BirthDate = new DateTime(1994, 3, 1),
                    Address = "Hotel Ana Giriş",
                    City = "İstanbul",
                    Country = "Türkiye",
                    Nationality = "T.C.",
                    CreatedDate = DateTime.Now,
                    Status = DataStatus.Inserted
                });

                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("📥 [MANUEL RESEPSİYONİST EKLENDİ] → receptionist@bilgehotel.com");
                Console.ResetColor();
            }

            // 👥 Müşteriler
            if (!customerExists)
            {
                for (int i = 1; i <= 10; i++)
                {
                    string firstName = faker.Name.FirstName();
                    string lastName = faker.Name.LastName();
                    string username = $"customer_{firstName.ToLower()}_{i}";
                    string email = faker.Internet.Email(firstName, lastName);

                    User user = new User
                    {
                        UserName = username,
                        Email = email,
                        IsActivated = true,
                        ActivationCode = Guid.NewGuid(),
                        CreatedDate = DateTime.Now,
                        Status = DataStatus.Inserted
                    };

                    IdentityResult manualResult = await _userManager.CreateAsync(user, "123Pa$$word!");
                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, "Customer");

                        AppRole? role = await _roleManager.FindByNameAsync("Customer");
                        if (role != null)
                        {
                            user.AppRoleId = role.Id;
                            await _userManager.UpdateAsync(user);
                        }

                        _context.UserProfiles.Add(new UserProfile
                        {
                            UserId = user.Id,
                            FirstName = firstName,
                            LastName = lastName,
                            PhoneNumber = faker.Phone.PhoneNumber("05#########"),
                            IdentityNumber = faker.Random.Replace("###########"),
                            BirthDate = faker.Date.Past(30, DateTime.Today.AddYears(-18)),
                            Address = faker.Address.FullAddress(),
                            City = faker.Address.City(),
                            Country = "Turkey",
                            Nationality = "T.C.",
                            CreatedDate = DateTime.Now,
                            Status = DataStatus.Inserted
                        });

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"👥 [MÜŞTERİ EKLENDİ] → {firstName} {lastName}");
                        Console.ResetColor();
                    }
                }
            }

            await _context.SaveChangesAsync();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("✅ AppUserSeeder tamamlandı!");
            Console.ResetColor();
        }
    }
}


// User sınıfı IdentityUser<int>'den miras alıyor. Bu nedenle kullanıcıları doğrudan context.Users.Add(user) şeklinde veritabanına atamıyoruz.

//👉 Bunun yerine UserManager kullanıyoruz:

