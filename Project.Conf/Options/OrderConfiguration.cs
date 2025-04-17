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
    public class OrderConfiguration:BaseConfiguration<Order>
    {
        public override void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id); // 🔑 Anahtar

            // 🧾 Sipariş Detayları
            builder.Property(o => o.TotalAmount)
                   .HasColumnType("decimal(10,2)")
                   .IsRequired();

            builder.Property(o => o.OrderDate)
                   .IsRequired();

            builder.Property(o => o.Status)
                   .IsRequired();

            builder.Property(o => o.OrderType)
                   .IsRequired();

            // 🔗 İlişkiler

            builder.HasOne(o => o.User)
                   .WithMany(u => u.Orders)
                   .HasForeignKey(o => o.UserId)
                   .OnDelete(DeleteBehavior.NoAction); // kullanıcı silinince sipariş kalabilir


            builder.HasOne(o => o.Reservation)
           .WithMany(r => r.Orders)
           .HasForeignKey(o => o.ReservationId)
           .OnDelete(DeleteBehavior.Restrict); // Rezervasyon silinirse siparişler de silinir

            builder.HasOne(o => o.Payment)
                   .WithMany(p => p.Orders)
                   .HasForeignKey(o => o.PaymentId)
                   .OnDelete(DeleteBehavior.SetNull); // Siparişin ödemesi varsa tutulur, yoksa null olabilir
        }
    }
}
