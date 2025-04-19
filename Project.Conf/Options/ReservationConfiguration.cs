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

    public class ReservationConfiguration:BaseConfiguration<Reservation>
    {
        public override void Configure(EntityTypeBuilder<Reservation> builder)
        {


            builder.HasKey(r => r.Id);

            builder.Property(r => r.TotalPrice)
                   .HasColumnType("decimal(10,2)");

            builder.Property(r => r.DiscountRate)
                   .HasColumnType("decimal(5,2)");

            builder.Property(r => r.CurrencyCode)
                   .HasMaxLength(10);

            builder.Property(r => r.ExchangeRate)
                   .HasColumnType("decimal(10,4)");

            builder.Property(r => r.CheckInTime)
                   .HasConversion(
                       v => v.ToString(),     // TimeSpan → string
                       v => TimeSpan.Parse(v) // string → TimeSpan
                   );

            builder.Property(r => r.Status)
                   .IsRequired();

            builder.Property(r => r.Package)
                   .IsRequired();

            // 🔗 İlişkiler

            builder.HasOne(r => r.User)
                   .WithMany(u => u.Reservations)
                   .HasForeignKey(r => r.UserId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(r => r.Customer)
                   .WithMany(c => c.Reservations)
                   .HasForeignKey(r => r.CustomerId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(r => r.Room)
                   .WithMany(rm => rm.Reservations)
                   .HasForeignKey(r => r.RoomId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.Campaign)
                   .WithMany(c => c.Reservations)
                   .HasForeignKey(r => r.CampaignId)
                   .OnDelete(DeleteBehavior.SetNull);

           

            builder.HasOne(r => r.EarlyReservationDiscount)
                   .WithOne(d => d.Reservation)
                   .HasForeignKey<EarlyReservationDiscount>(d => d.ReservationId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(r => r.ExtraExpenses)
                   .WithOne(e => e.Reservation)
                   .HasForeignKey(e => e.ReservationId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(r => r.Orders)
                   .WithOne(o => o.Reservation)
                   .HasForeignKey(o => o.ReservationId)
                   .OnDelete(DeleteBehavior.Cascade);

           
        }
    }
}
