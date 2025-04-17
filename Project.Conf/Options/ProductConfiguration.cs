using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Conf.Options
{
    public class ProductConfiguration:BaseConfiguration<Product>
    {
        public override void Configure(EntityTypeBuilder<Product> builder)
        {

            builder.HasKey(p => p.Id); // 🔑 Primary key

            // 📦 Ürün bilgileri
            builder.Property(p => p.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(p => p.Description)
                   .HasMaxLength(300); // 📝 Opsiyonel açıklama

            builder.Property(p => p.Price)
                   .HasColumnType("decimal(10,2)")
                   .IsRequired();

            builder.Property(p => p.ImagePath)
                   .HasMaxLength(255); // 🌄 Ürün görseli

            builder.Property(p => p.IsInStock)
                   .IsRequired();

            builder.Property(p => p.Category)
                   .IsRequired();

            // 🔗 İlişkiler

            builder.HasMany(p => p.ExtraExpenses)
                   .WithOne(e => e.Product)
                   .HasForeignKey(e => e.ProductId)
                   .OnDelete(DeleteBehavior.Restrict); // Ürün silinmeden önce bağımlı harcamalar temizlenmeli

            builder.HasMany(p => p.OrderDetails)
                   .WithOne(od => od.Product)
                   .HasForeignKey(od => od.ProductId)
                   .OnDelete(DeleteBehavior.Restrict); // Aynı şekilde, sipariş detayları varsa ürün silinemez
        }
    }
}
