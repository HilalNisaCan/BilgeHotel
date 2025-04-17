using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Entities.Enums;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Project.Conf.Options
{
    public class RoomMaintenanceAssignmentConfiguration:BaseConfiguration<RoomMaintenanceAssignment>
    {
        public override void Configure(EntityTypeBuilder<RoomMaintenanceAssignment> builder)
        {
            builder.HasKey(a => a.Id); // 🔑 Primary Key

            // 📝 Açıklama ve durum
            builder.Property(a => a.Description)
                   .HasMaxLength(300); // Görev notu / açıklama

            builder.Property(a => a.MaintenanceStatus)
                   .IsRequired(); // Enum: Atandı, Tamamlandı, İptal vb.

            builder.Property(a => a.AssignedDate)
                   .IsRequired(); // Ne zaman atandı?

            // 🔗 İlişkiler

            builder.HasOne(x => x.Room)
           .WithMany(x => x.MaintenanceAssignments)
           .HasForeignKey(x => x.RoomId)
           .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(a => a.Employee)
                   .WithMany(e => e.MaintenanceAssignments)
                   .HasForeignKey(a => a.EmployeeId)
                   .OnDelete(DeleteBehavior.Restrict); // Çalışan silinirse atamaları da gider

            builder.HasOne(a => a.RoomMaintenance)
                   .WithMany(m => m.MaintenanceAssignments)
                   .HasForeignKey(a => a.RoomMaintenanceId)
                   .OnDelete(DeleteBehavior.Restrict); // Bakım silinirse atamalar da gider


        }
    }
}
