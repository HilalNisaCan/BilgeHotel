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
    public class EmployeeShiftAssignmentConfiguration:BaseConfiguration<EmployeeShiftAssignment>
    {
        public override void Configure(EntityTypeBuilder<EmployeeShiftAssignment> builder)
        {
            builder.HasKey(e => e.Id); // 🔑 Primary key

            // 📋 Atama bilgileri
            builder.Property(e => e.AssignedDate)
                   .IsRequired(); // Ne zaman bu vardiyaya atandı?

            builder.Property(e => e.Status)
                   .IsRequired(); // Enum: Assigned, Completed, Cancelled vb.

            builder.Property(e => e.Description)
                   .HasMaxLength(300); // Açıklama / not

            // 🔗 İlişkiler

            builder.HasOne(e => e.Employee)
                   .WithMany(emp => emp.EmployeeShiftAssignments)
                   .HasForeignKey(e => e.EmployeeId)
                   .OnDelete(DeleteBehavior.Cascade); // Çalışan silinirse atamaları da gider

            builder.HasOne(e => e.EmployeeShift)
                   .WithMany(shift => shift.ShiftAssignments)
                   .HasForeignKey(e => e.EmployeeShiftId)
                   .OnDelete(DeleteBehavior.Cascade); // Vardiya silinirse atamalar da silinsin


        }
    }
}
