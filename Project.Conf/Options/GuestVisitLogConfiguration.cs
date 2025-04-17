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
    public class GuestVisitLogConfiguration:BaseConfiguration<GuestVisitLog>
    {
        public override void Configure(EntityTypeBuilder<GuestVisitLog> builder)
        {

            builder.Property(g => g.GuestNationality).HasMaxLength(50);
            builder.Property(g => g.BirthDate).IsRequired();
            builder.Property(g => g.EntryDate).IsRequired();
            builder.Property(g => g.ExitDate);

            builder.HasOne(g => g.Customer)
                   .WithMany(c => c.GuestVisitLogs)
                   .HasForeignKey(g => g.CustomerId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(g => g.Room)
                   .WithMany(r => r.GuestVisitLogs)
                   .HasForeignKey(g => g.RoomId)
                   .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
