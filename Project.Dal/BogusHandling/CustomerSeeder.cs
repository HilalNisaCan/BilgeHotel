using Bogus;
using Project.Dal.ContextClasses;
using Project.Entities.Enums;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Dal.BogusHandling
{

    /// <summary>
    /// CustomerSeeder sınıfı, sahte müşteri (Customer) verisi üretmek için kullanılır.
    /// Bu sınıf, User tablosuna bağlı müşteri kayıtlarını oluşturur.
    /// Her kullanıcı (UserId) için yalnızca bir müşteri oluşturularak veri tekrarı engellenir.

    /// <summary>
    /// CustomerSeeder sınıfı, sadece rolü 'Customer' olan kullanıcılar için müşteri verisi oluşturur.
    /// Böylece personel ya da yönetici gibi sistem kullanıcılarına yanlışlıkla müşteri kaydı açılmaz.
    /// </summary>
    public static class CustomerSeeder
    {
        /// <summary>
        /// Eğer Customer tablosu boşsa, sadece Role'ü Customer olan AppUser'lara bağlı müşteri kayıtları üretir.
        /// </summary>
        public static async Task SeedAsync(MyContext context)
        {
            if (!context.Customers.Any())
            {
                List<int> customerUserIds = context.Users
                                                   .Where(u => u.Role == UserRole.Customer)
                                                   .Select(u => u.Id)
                                                   .ToList();

                List<Customer> customers = GenerateCustomers(customerUserIds);
                context.Customers.AddRange(customers);
                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Customer rolüne sahip kullanıcı ID’lerine bağlı sahte müşteri oluşturur.
        /// </summary>
        public static List<Customer> GenerateCustomers(List<int> userIds)
        {
            Faker faker = new Faker("en");
            List<Customer> customers = new List<Customer>();

            foreach (int userId in userIds)
            {
                Customer customer = new Customer
                {
                    UserId = userId,
                    FirstName = faker.Name.FirstName(),
                    LastName = faker.Name.LastName(),
                    IdentityNumber = faker.Random.Replace("###########"),
                    PhoneNumber = faker.Phone.PhoneNumber("5##-###-####"),
                    LoyaltyPoints = faker.Random.Int(0, 100),
                    BillingDetails = faker.Address.FullAddress(),
                    IsIdentityVerified = faker.Random.Bool(),
                    NeedsIdentityCheck = faker.Random.Bool(),
                    CreatedDate = DateTime.Now,
                    Status = DataStatus.Inserted
                };

                customers.Add(customer);
            }

            return customers;
        }
    }
}

    /*
    💡 Açıklamalar:
    UserId: Var olan User tablosundaki Id listesinden rastgele seçilir.

    IdentityNumber: Türkiye kimlik numarası formatına uygun (11 haneli) rastgele sayı.

    LoyaltyPoints: Müşteriye özel puan sistemi (0–10).

    IsIdentityVerified / NeedsIdentityCheck: Gerçek kimlik doğrulama işlemleri için kullanılabilir.

    DataStatus.Active: Bizim enum yapımıza uygun aktif durum bilgisi.

   bu sınıf, sisteme kayıtlı AppUser'lara bağlı olarak sahte müşteriler üretir.
  Kimlik numarası, fatura bilgisi, sadakat puanı gibi alanlar gerçekçi şekilde faker ile üretilmiştir.

    */


