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
    public class RoomConfiguration : BaseConfiguration<Room>
    {
        public override void Configure(EntityTypeBuilder<Room> builder)
        {


            builder.HasKey(r => r.Id); // Anahtar

            builder.Property(r => r.RoomNumber)
                   .IsRequired()
                   .HasMaxLength(10);

            builder.Property(r => r.FloorNumber)
                   .IsRequired();

            builder.Property(r => r.PricePerNight)
                   .HasColumnType("decimal(10,2)");

            builder.Property(r => r.Status)
                   .IsRequired();

            builder.Property(r => r.Capacity)
                   .IsRequired();

            builder.Property(r => r.HasAirConditioning).HasDefaultValue(true);
            builder.Property(r => r.HasHairDryer).HasDefaultValue(true);
            builder.Property(r => r.HasTV).HasDefaultValue(true);
            builder.Property(r => r.HasWirelessInternet).HasDefaultValue(true);

            // İlişkiler

            builder.HasMany(r => r.Reservations)
                   .WithOne(rz => rz.Room)
                   .HasForeignKey(rz => rz.RoomId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(r => r.RoomImages)
                   .WithOne(ri => ri.Room)
                   .HasForeignKey(ri => ri.RoomId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(r => r.RoomMaintenance)
                   .WithOne(m => m.Room)
                   .HasForeignKey(m => m.RoomId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(r => r.CleaningSchedules)
                   .WithOne(cs => cs.Room)
                   .HasForeignKey(cs => cs.RoomId)
                   .OnDelete(DeleteBehavior.Cascade);

           

            builder.HasMany(r => r.GuestVisitLogs)
                   .WithOne(g => g.Room)
                   .HasForeignKey(g => g.RoomId)
                   .OnDelete(DeleteBehavior.NoAction);




         }
    }
}
