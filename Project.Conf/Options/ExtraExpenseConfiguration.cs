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
    public class ExtraExpenseConfiguration:BaseConfiguration<ExtraExpense>
    {
        public override void Configure(EntityTypeBuilder<ExtraExpense> builder)
        {
            builder.HasKey(e => e.Id); // 🔑 Anahtar

            // 📄 Açıklama alanı (detay bilgi)
            builder.Property(e => e.Description)
                   .HasMaxLength(300);

            // 💰 Fiyatlandırma alanları
            builder.Property(e => e.UnitPrice)
                   .HasColumnType("decimal(10,2)");

            builder.Property(e => e.Quantity)
                   .IsRequired();

            builder.Property(e => e.ExpenseDate)
                   .IsRequired();

            // 🔗 İlişkiler

            builder.HasOne(e => e.Customer)
                   .WithMany(c => c.ExtraExpenses)
                   .HasForeignKey(e => e.CustomerId)
                   .OnDelete(DeleteBehavior.Cascade); // Müşteri silinirse harcamaları da gider

            builder.HasOne(e => e.Reservation)
                   .WithMany(r => r.ExtraExpenses)
                   .HasForeignKey(e => e.ReservationId)
                   .OnDelete(DeleteBehavior.Restrict); // Rezervasyonla birlikte silinsin

            builder.HasOne(e => e.Product)
                   .WithMany(p => p.ExtraExpenses)
                   .HasForeignKey(e => e.ProductId)
                   .OnDelete(DeleteBehavior.Restrict); // Ürün silinemez, önce harcamalar silinmeli

            builder.HasOne(e => e.Payment)
                   .WithMany(p => p.ExtraExpenses)
                   .HasForeignKey(e => e.PaymentId)
                   .OnDelete(DeleteBehavior.Restrict); // Harcama ödeme olmadan da kalabilir
        }
    }
}
