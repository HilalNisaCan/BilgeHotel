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
    public class RoomCleaningScheduleConfiguration:BaseConfiguration<RoomCleaningSchedule>
    {
        public override void Configure(EntityTypeBuilder<RoomCleaningSchedule> builder)
        {
            builder.HasKey(c => c.Id); // 🔑 Primary key

            // 🧼 Temizlik bilgileri
            builder.Property(c => c.ScheduledDate)
                   .IsRequired(); // Temizlik günü

            builder.Property(c => c.Status)
                   .IsRequired(); // Temizlik durumu (enum)

            builder.Property(c => c.Description)
                   .HasMaxLength(300); // Açıklama / Not

            // 🔗 İlişkiler

            builder.HasOne(c => c.Room)
                   .WithMany(r => r.CleaningSchedules)
                   .HasForeignKey(c => c.RoomId)
                   .OnDelete(DeleteBehavior.Cascade); // Oda silinirse planlar da silinsin

            builder.HasOne(x => x.AssignedEmployee)
           .WithMany(x => x.RoomCleaningSchedules)
           .HasForeignKey(x => x.AssignedEmployeeId)
         .OnDelete(DeleteBehavior.SetNull); // 💣 burası önemli! // Çalışan silinirse temizlik planı kalsın



        }
    }
}
