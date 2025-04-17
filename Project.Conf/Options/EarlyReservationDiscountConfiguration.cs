using Microsoft.EntityFrameworkCore;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Conf.Options
{
    public class EarlyReservationDiscountConfiguration : BaseConfiguration<EarlyReservationDiscount>
    {
        public override void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<EarlyReservationDiscount> builder)
        {
            builder.HasKey(e => e.Id); // 🔑 Anahtar

            builder.Property(e => e.DiscountPercentage)
                   .HasColumnType("decimal(5,2)")
                   .IsRequired();

            builder.Property(e => e.ValidityDays)
                   .IsRequired();

            builder.Property(e => e.AppliedDate)
                   .IsRequired();

            builder.Property(e => e.DiscountType)
                   .IsRequired();

            // 🔗 İlişkiler

            builder.HasOne(e => e.Reservation)
                   .WithOne(r => r.EarlyReservationDiscount)
                   .HasForeignKey<EarlyReservationDiscount>(e => e.ReservationId)
                   .OnDelete(DeleteBehavior.Restrict); // Rezervasyon silinirse indirim de gider

            builder.HasOne(e => e.Customer)
                   .WithMany(c => c.EarlyReservationDiscounts)
                   .HasForeignKey(e => e.CustomerId)
                   .OnDelete(DeleteBehavior.Cascade); // Müşteri silinirse indirimler de gider

            builder.HasOne(e => e.Campaign)
                   .WithMany(c => c.EarlyReservationDiscounts)
                   .HasForeignKey(e => e.CampaignId)
                   .OnDelete(DeleteBehavior.SetNull); // Kampanya silinse de indirim kalabilir
        }

 }   }

