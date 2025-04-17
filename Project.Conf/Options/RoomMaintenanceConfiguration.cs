using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Entities.Enums;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Conf.Options
{
    public class RoomMaintenanceConfiguration:BaseConfiguration<RoomMaintenance>
    {
        public override void Configure(EntityTypeBuilder<RoomMaintenance> builder)
        {

            builder.HasKey(m => m.Id);

            builder.Property(m => m.Description)
                   .HasMaxLength(300);

            builder.Property(m => m.MaintenanceType)
                   .IsRequired();

            builder.Property(m => m.Status)
                   .IsRequired();

            builder.Property(m => m.StartDate)
                   .IsRequired();

            builder.Property(m => m.EndDate);

            builder.Property(m => m.ScheduledDate)
                   .IsRequired();

            builder.HasOne(m => m.Room)
                   .WithMany(r => r.RoomMaintenance)
                   .HasForeignKey(m => m.RoomId)
                   .OnDelete(DeleteBehavior.Cascade);


        }
    }
}
