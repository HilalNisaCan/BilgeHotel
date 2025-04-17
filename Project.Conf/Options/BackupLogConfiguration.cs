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
    public class BackupLogConfiguration:BaseConfiguration<BackupLog>
    {
        public override void Configure(EntityTypeBuilder<BackupLog> builder)
        {

            builder.HasKey(b => b.Id); // 🔑 Anahtar

            // 💽 Log alanları
            builder.Property(b => b.FilePath)
                   .IsRequired()
                   .HasMaxLength(255); // Yedek yolu

            builder.Property(b => b.IsAuthorized)
                   .IsRequired(); // Yetkili mi?

            builder.Property(b => b.IsRestored)
                   .IsRequired();

            builder.Property(b => b.BackupDate)
                   .IsRequired();

            builder.Property(b => b.RestoredDate); // Nullable

            builder.Property(b => b.Status)
                   .IsRequired();

            builder.Property(b => b.ErrorMessage)
                   .HasMaxLength(500);

            builder.Property(b => b.IPAddress)
                   .HasMaxLength(45); // IPv6 destekler

            builder.Property(b => b.MachineName)
                   .HasMaxLength(100);

            // 🔗 İlişki: Yedek alan kullanıcı
            builder.HasOne(b => b.User)
                   .WithMany(u => u.BackupLogs)
                   .HasForeignKey(b => b.UserId)
                   .OnDelete(DeleteBehavior.Restrict); // Kullanıcı silinemezse iyi olur

        }
    }
}
