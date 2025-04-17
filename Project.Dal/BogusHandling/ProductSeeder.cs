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
    /// Ürün (Product) tablosuna sahte veriler eklemek için kullanılan seeder sınıfıdır.
    /// Ürün adı, fiyatı, kategori, açıklama gibi alanları Faker kütüphanesi ile doldurur.
    /// </summary>
    public static class ProductSeeder
    {

        /// <summary>
        /// Eğer veritabanında ürün yoksa, sahte ürünlerden oluşan bir liste üretir ve ekler.
        /// </summary>
        /// <param name="context">Veritabanı bağlamı</param>
        public static async Task SeedAsync(MyContext context)
        {
            if (!context.Products.Any())
            {
                List<Product> fakeProducts = GenerateFakeProducts(20);
                context.Products.AddRange(fakeProducts);
                await context.SaveChangesAsync();
            }
        }



        /// <summary>
        /// Rastgele ürünlerden oluşan bir liste üretir.
        /// </summary>
        /// <param name="count">Kaç adet ürün üretileceği (varsayılan: 20)</param>
        /// <returns>Ürün listesi</returns>
        public static List<Product> GenerateFakeProducts(int count = 20)
        {
            Faker<Product> faker = new Faker<Product>("en")

                // Ürün adı (örnek: Laptop, T-shirt, Şampuan vb.)
                .RuleFor(p => p.Name, f => f.Commerce.ProductName())

                // Fiyat (20 TL ile 500 TL arasında)
                .RuleFor(p => p.Price, f => f.Random.Decimal(20, 500))

                // Ürün kategorisi (Minibar, Spa, Restaurant gibi)
                .RuleFor(p => p.Category, f => f.PickRandom<ProductCategory>())

                // Stokta mı? true/false
                .RuleFor(p => p.IsInStock, f => f.Random.Bool())

                // Ürün açıklaması (otomatik kısa metin)
                .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())

                // Görsel yolu (örnek görsel url)
                .RuleFor(p => p.ImagePath, f => f.Image.PicsumUrl())

                // Oluşturulma tarihi
                .RuleFor(p => p.CreatedDate, f => f.Date.Past())

                // Ürün durumu (enum'dan rastgele)
                .RuleFor(p => p.ProductStatus,  f => f.PickRandom<ProductStatus>())

                // Güncelleme ve silinme tarihi null
                .RuleFor(p => p.ModifiedDate, f => null)
                .RuleFor(p => p.DeletedDate, f => null);

            // Belirtilen sayıda ürün oluşturulup döndürülür
            List<Product> productList = faker.Generate(count);
            return productList;
        }
    }

 /*   “ Bu sınıf Product tablosu için sahte ürünler üretiyor.
Kategoriler enum üzerinden sabitlendi, fiyat, açıklama, stok bilgisi gibi alanlar Faker ile dinamik üretildi.
”*/
}
