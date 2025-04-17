using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Conf.Options
{
    // User Configuration
    public class UserConfiguration : BaseConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id); // 🔑 IdentityUser'dan gelir

            // 🛡️ Güvenlik ve hesap bilgileri
            builder.Property(u => u.ActivationCode)
                   .IsRequired();

            builder.Property(u => u.CreatedDate)
                   .IsRequired();

            builder.Property(u => u.IsActivated)
                   .IsRequired();

            builder.Property(u => u.Role)
                   .IsRequired(); // Enum: Admin, Receptionist, Customer vs.

            // 🔗 İlişkiler

            builder.HasOne(u => u.UserProfile)
                   .WithOne(up => up.User)
                   .HasForeignKey<UserProfile>(up => up.UserId)
                   .OnDelete(DeleteBehavior.Cascade); // Kullanıcı silinirse profili de gider

            builder.HasMany(u => u.Reservations)
                   .WithOne(r => r.User)
                   .HasForeignKey(r => r.UserId)
                   .OnDelete(DeleteBehavior.SetNull); // Kullanıcı silinse bile rezervasyon kalsın

            builder.HasMany(u => u.Payments)
            .WithOne(p => p.User)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.ReportLogs)
                   .WithOne(r => r.User)
                   .HasForeignKey(r => r.UserId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(u => u.BackupLogs)
                   .WithOne(b => b.User)
                   .HasForeignKey(b => b.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.Reviews)
                   .WithOne(r => r.User)
                   .HasForeignKey(r => r.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.Orders)
                   .WithOne(o => o.User)
                   .HasForeignKey(o => o.UserId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
