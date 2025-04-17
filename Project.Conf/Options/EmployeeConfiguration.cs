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
    public class EmployeeConfiguration:BaseConfiguration<Employee>
    {
        public override void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasKey(e => e.Id); // 🔑 Primary key

            // 📇 Kişisel bilgiler
            builder.Property(e => e.FirstName)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(e => e.LastName)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(e => e.PhoneNumber)
                   .HasMaxLength(20);

            builder.Property(e => e.Address)
                   .HasMaxLength(250);

            builder.Property(e => e.IdentityNumber)
                   .HasMaxLength(11); // TC Kimlik no

            builder.Property(e => e.IsActive)
                   .IsRequired();

            builder.Property(e => e.HireDate)
                   .IsRequired();

            // 🧾 Maaş ve pozisyon
            builder.Property(e => e.Position).IsRequired();

            builder.Property(e => e.SalaryType).IsRequired();

            builder.Property(e => e.HourlyWage)
                   .HasColumnType("decimal(10,2)")
                   .IsRequired();

            builder.Property(e => e.MonthlySalary)
                   .HasColumnType("decimal(10,2)")
                   .IsRequired();

            builder.Property(e => e.HasOvertime)
                   .IsRequired();

            builder.Property(e => e.WeeklyWorkedHours)
                   .IsRequired();

            builder.Property(e => e.TotalWorkedHours)
                   .IsRequired();

            builder.Property(e => e.ShiftStart)
                   .IsRequired();

            builder.Property(e => e.ShiftEnd)
                   .IsRequired();

            // 🔗 İlişkiler

            builder.HasMany(e => e.EmployeeShiftAssignments)
                   .WithOne(a => a.Employee)
                   .HasForeignKey(a => a.EmployeeId)
                   .OnDelete(DeleteBehavior.Cascade); // Çalışan silinirse atamaları da silinir

            builder.HasMany(e => e.MaintenanceAssignments)
                   .WithOne(a => a.Employee)
                   .HasForeignKey(a => a.EmployeeId)
                   .OnDelete(DeleteBehavior.Cascade); // Bakım görevleri de silinsin

            builder.HasMany(e => e.RoomCleaningSchedules)
                   .WithOne(c => c.AssignedEmployee)
                   .HasForeignKey(c => c.AssignedEmployeeId)
                   .OnDelete(DeleteBehavior.SetNull); // Temizlik planı kalsın ama çalışan silinebilir
        }
    }
}
