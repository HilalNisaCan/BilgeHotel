using Bogus;
using Microsoft.EntityFrameworkCore;
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
    /// Sipariş (Order) verilerini sahte olarak üretip veritabanına ekleyen seeder sınıfıdır.
    /// Her müşteri için rastgele 3 sipariş üretilir.
    /// </summary>
    public static class OrderSeeder
    {
        public static async Task SeedAsync(MyContext context)
        {
            if (context.Orders.Any()) return;

            List<Payment> payments = await context.Payments
             .Include(p => p.Customer)
             .ToListAsync();

            List<Product> products = await context.Products.ToListAsync();
            Faker faker = new Faker("en");
            List<Order> orders = new List<Order>();

            if (!payments.Any() || !products.Any())
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("❌ [OrderSeeder] Gerekli Payment veya Product verisi yok.");
                Console.ResetColor();
                return;
            }

            foreach (Payment payment in payments)
            {
              
                if (payment.Customer == null || payment.Customer.UserId == null)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"⚠️ [OrderSeeder] PaymentId {payment.Id} için geçerli Customer veya UserId yok. Atlaniyor.");
                    Console.ResetColor();
                    continue;
                }


                int orderCount = faker.Random.Int(1, 3); // Her ödeme için 1-3 arası sipariş

                for (int i = 0; i < orderCount; i++)
                {
                    Order order = new Order
                    {
                        UserId = payment.Customer.UserId.Value,
                        ReservationId = payment.ReservationId,
                        PaymentId = payment.Id,
                        OrderDate = faker.Date.BetweenOffset(DateTime.Now.AddDays(-30), DateTime.Now).DateTime,
                        OrderType = faker.PickRandom<OrderType>(), // Enum varsa
                        OrderStatus = OrderStatus.Preparing,
                        TotalAmount = 0, // Detaylarla doldurulacak
                        CreatedDate = DateTime.Now,
                        Status = DataStatus.Inserted
                    };

                    // Order detayları eklenecek
                    int itemCount = faker.Random.Int(1, 4);
                    List<OrderDetail> details = new List<OrderDetail>();
                    decimal total = 0;

                    for (int j = 0; j < itemCount; j++)
                    {
                        Product product = faker.PickRandom(products);
                        int quantity = faker.Random.Int(1, 3);
                        decimal lineTotal = product.Price * quantity;
                        total += lineTotal;

                        details.Add(new OrderDetail
                        {
                            ProductId = product.Id,
                            Quantity = quantity,
                            UnitPrice = product.Price,
                            CreatedDate = DateTime.Now,
                            Status = DataStatus.Inserted
                        });
                    }

                    order.TotalAmount = total;
                    order.OrderDetails = details;

                    orders.Add(order);
                }
            }

            context.Orders.AddRange(orders);
            await context.SaveChangesAsync();
        }
    }


    //“ bu sınıf, her müşteri için rastgele 3 sipariş üretir. Sipariş tipi, durumu, tutarı gibi alanlar faker ile anlamlı biçimde doldurulur.
    //Bu yapı ileride ürün detayları veya sipariş analizleri için kolayca genişletilebilir.


    /*
     * Açıklamalar:
Bu seed, her müşteri için rastgele 3 sipariş ekler.

Sipariş türü, durumu, toplam miktar gibi veriler rastgele oluşturulur.

OrderType ve OrderStatus enum'ları kullanılacaktır.

*/
}
