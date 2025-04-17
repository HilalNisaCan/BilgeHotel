using Bogus;
using Microsoft.AspNetCore.Identity;
using Project.Dal.ContextClasses;
using Project.Entities.Enums;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Dal.BogusHandling
{/// <summary>
 /// Sisteme giriş yapacak kullanıcıları (yöneticiler, resepsiyonistler ve müşteriler) oluşturur.
 /// Console çıktıları ile hangi kullanıcıların başarıyla eklendiği veya hata verdiği loglanır.
 /// </summary>
    public class AppUserSeeder
    {
        private readonly UserManager<User> _userManager;
        private readonly MyContext _context;

        public AppUserSeeder(UserManager<User> userManager, MyContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task SeedAsync(List<Employee> employees)
        {
            bool adminVarMi = _context.Users.Any(u => u.Role != UserRole.Customer);
            bool customerVarMi = _context.Users.Any(u => u.Role == UserRole.Customer);

            Faker faker = new Faker("en");
            List<User> createdUsers = new List<User>();

            // --- ADMİN + RESEPSİYONİST EKLE ---
            if (!adminVarMi)
            {
                List<(string FullName, string Username, string Email, UserRole Role)> adminList = new List<(string, string, string, UserRole)>
            {
                ("Selahattin Alkomut", "salkomut", "salkomut@bilgehotel.com", UserRole.HR),
                ("Levent Sişarpsoy", "lsi_satis", "levent@bilgehotel.com", UserRole.Staff),
                ("Gülay Aydınlık", "gaydinlik", "gulay@bilgehotel.com", UserRole.ReceptionChief),
                ("Selahattin Karadibag", "skaradibag", "it@bilgehotel.com", UserRole.IT)
            };

                for (int i = 0; i < adminList.Count; i++)
                {
                    var admin = adminList[i];
                    User user = new User
                    {
                        UserName = admin.Username,
                        Email = admin.Email,
                        Role = admin.Role,
                        IsActivated = true,
                        ActivationCode = Guid.NewGuid(),
                        CreatedDate = DateTime.Now,
                        Status = DataStatus.Inserted
                    };

                    IdentityResult result = await _userManager.CreateAsync(user, "123Pa$$word!");

                    if (result.Succeeded)
                    {
                        createdUsers.Add(user);
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
                                City = "İstanbul",
                                Country = "Türkiye",
                                Nationality = "T.C.",
                                CreatedDate = DateTime.Now,
                                Status = DataStatus.Inserted
                            });
                        }

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"✔️ [YÖNETİCİ EKLENDİ] → {admin.FullName} | Kullanıcı Adı: {admin.Username}");
                        Console.ResetColor();
                    }
                }

                List<Employee> receptionists = employees.Where(e => e.Position == EmployeePosition.Receptionist).ToList();

                foreach (Employee receptionist in receptionists)
                {
                    string username = faker.Internet.UserName(receptionist.FirstName, receptionist.LastName);
                    string email = faker.Internet.Email(receptionist.FirstName, receptionist.LastName);

                    User user = new User
                    {
                        UserName = username,
                        Email = email,
                        Role = UserRole.Receptionist,
                        IsActivated = true,
                        ActivationCode = Guid.NewGuid(),
                        CreatedDate = DateTime.Now,
                        Status = DataStatus.Inserted
                    };

                    IdentityResult result = await _userManager.CreateAsync(user, "123Pa$$word!");

                    if (result.Succeeded)
                    {
                        createdUsers.Add(user);
                        receptionist.UserId = user.Id;

                        _context.UserProfiles.Add(new UserProfile
                        {
                            UserId = user.Id,
                            FirstName = receptionist.FirstName,
                            LastName = receptionist.LastName,
                            PhoneNumber = receptionist.PhoneNumber,
                            IdentityNumber = faker.Random.Replace("###########"),
                            BirthDate = new DateTime(1995, 5, 15),
                            Address = "Otel Resepsiyonu",
                            City = "İstanbul",
                            Country = "Türkiye",
                            Nationality = "T.C.",
                            CreatedDate = DateTime.Now,
                            Status = DataStatus.Inserted
                        });

                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine($"👩‍💼 [RESEPSİYONİST EKLENDİ] → {receptionist.FirstName} {receptionist.LastName} | Kullanıcı Adı: {username}");
                        Console.ResetColor();
                    }
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("⚠️ Yönetici ve resepsiyonist kullanıcılar zaten var. Bu kısım atlandı.");
                Console.ResetColor();
            }

            // --- MÜŞTERİ EKLE ---
            // --- MÜŞTERİ EKLE ---
            if (!customerVarMi)
            {
                for (int i = 1; i <= 10; i++)
                {
                    string firstName = faker.Name.FirstName();
                    string lastName = faker.Name.LastName();
                    string username = faker.Internet.UserName(firstName, lastName);
                    string email = faker.Internet.Email(firstName, lastName);

                    User user = new User
                    {
                        UserName = username,
                        Email = email,
                        Role = UserRole.Customer,
                        IsActivated = true,
                        ActivationCode = Guid.NewGuid(),
                        CreatedDate = DateTime.Now,
                        Status = DataStatus.Inserted
                    };

                    IdentityResult result = await _userManager.CreateAsync(user, "123Pa$$word!");

                    if (result.Succeeded)
                    {
                        createdUsers.Add(user);

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
                            Country = "Türkiye",
                            Nationality = "T.C.",
                            CreatedDate = DateTime.Now,
                            Status = DataStatus.Inserted
                        });

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"👥 [MÜŞTERİ EKLENDİ] → {firstName} {lastName} | Kullanıcı Adı: {username}");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"❌ [MÜŞTERİ HATASI] → {firstName} {lastName} | Hatalar: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                        Console.ResetColor();
                    }
                }
            }

            await _context.SaveChangesAsync();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("✅ Tüm kullanıcılar ve eşleştirmeler başarıyla kaydedildi.");
            Console.ResetColor();
        }
    }
   }

// User sınıfı IdentityUser<int>'den miras alıyor. Bu nedenle kullanıcıları doğrudan context.Users.Add(user) şeklinde veritabanına atamıyoruz.

//👉 Bunun yerine UserManager kullanıyoruz:

