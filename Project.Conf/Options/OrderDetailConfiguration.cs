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
    public class OrderDetailConfiguration:BaseConfiguration<OrderDetail>
    {
        public override void Configure(EntityTypeBuilder<OrderDetail> builder)
        {

            builder.HasKey(od => od.Id); // 🔑 Primary key

            // 📦 Sipariş detayı
            builder.Property(od => od.Quantity)
                   .IsRequired();

            builder.Property(od => od.UnitPrice)
                   .HasColumnType("decimal(10,2)")
                   .IsRequired();

            // 🔗 İlişkiler

            builder.HasOne(od => od.Order)
                   .WithMany(o => o.OrderDetails)
                   .HasForeignKey(od => od.OrderId)
                   .OnDelete(DeleteBehavior.Cascade); // Sipariş silinirse detayları da silinsin

            builder.HasOne(od => od.Product)
                   .WithMany(p => p.OrderDetails)
                   .HasForeignKey(od => od.ProductId)
                   .OnDelete(DeleteBehavior.Restrict); // Ürün varsa sipariş detayları silinmeden ürün silinemez
        }
    }
}
