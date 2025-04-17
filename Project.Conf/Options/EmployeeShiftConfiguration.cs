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
    public class EmployeeShiftConfiguration:BaseConfiguration<EmployeeShift>
    {
        public override void Configure(EntityTypeBuilder<EmployeeShift> builder)
        {
            builder.HasKey(s => s.Id); // 🔑 Primary key

            // 🕐 Zaman aralığı
            builder.Property(s => s.ShiftStart)
                   .IsRequired(); // Vardiya başlangıç saati

            builder.Property(s => s.ShiftEnd)
                   .IsRequired(); // Vardiya bitiş saati


            builder.Property(s => s.Description)
                   .HasMaxLength(300); // Açıklama

            builder.Property(e => e.OvertimePay)
           .HasColumnType("decimal(10,2)")
           .IsRequired();

            // 🔗 İlişkiler

            builder.HasMany(s => s.ShiftAssignments)
                   .WithOne(a => a.EmployeeShift)
                   .HasForeignKey(a => a.EmployeeShiftId)
                   .OnDelete(DeleteBehavior.Cascade); // Vardiya silinirse atamalar da silinir

        }
    }
}
