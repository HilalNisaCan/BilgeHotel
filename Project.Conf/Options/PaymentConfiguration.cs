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
    public class PaymentConfiguration:BaseConfiguration<Payment>
    {
        public override void Configure(EntityTypeBuilder<Payment> builder)
        {

            builder.HasKey(p => p.Id); // 🔑 Primary key

            // 💵 Fiyat alanları
            builder.Property(p => p.TotalAmount)
                   .HasColumnType("decimal(10,2)");

            builder.Property(p => p.PaidAmount)
                   .HasColumnType("decimal(10,2)");

            builder.Property(p => p.ExchangeRate)
                   .HasColumnType("decimal(10,4)");

            builder.Property(p => p.PaymentMethod)
                   .IsRequired();

            builder.Property(p => p.PaymentStatus)
                   .IsRequired();

            builder.Property(p => p.InvoiceNumber)
                   .HasMaxLength(50); // 🔢 Fatura numarası sınırlı olsun

            builder.Property(p => p.Description)
                   .HasMaxLength(500); // 📝 Açıklama uzun olabilir ama sınırlı

            builder.Property(p => p.TransactionId)
                   .HasMaxLength(100); // 💳 Banka işlem ID’si

            builder.Property(p => p.CancellationReason)
                   .HasMaxLength(300); // ❌ Ödeme iptal açıklaması

            builder.Property(p => p.IsAdvancePayment)
                   .IsRequired();

            builder.Property(p => p.IsRefunded)
                   .IsRequired();

            builder.Property(p => p.LastUpdated)
                   .IsRequired();

            // 🔗 İlişkiler

            builder.HasOne(p => p.Customer)
           .WithMany(c => c.Payments)
           .HasForeignKey(p => p.CustomerId)
           .OnDelete(DeleteBehavior.NoAction);

            builder.Property(p => p.UserId)
            .IsRequired(false);


            // Kullanıcı silinse bile ödeme kalsın

            builder.HasOne(p => p.Reservation)
                   .WithOne(r => r.Payment)
                   .HasForeignKey<Payment>(p => p.ReservationId)
                   .OnDelete(DeleteBehavior.Cascade); // Rezervasyon silinirse ödeme de gitsin

            builder.HasMany(p => p.ExtraExpenses)
                   .WithOne(e => e.Payment)
                   .HasForeignKey(e => e.PaymentId)
                   .OnDelete(DeleteBehavior.SetNull); // Harcama bağı kopar ama silinmez

        }
    }
}
