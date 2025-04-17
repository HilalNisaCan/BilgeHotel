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
    public class ReportLogConfiguration:BaseConfiguration<ReportLog>
    {
        public override void Configure(EntityTypeBuilder<ReportLog> builder)
        {
            builder.HasKey(r => r.Id); // 🔑 Anahtar

            // 🧠 Log bilgileri
            builder.Property(r => r.LogMessage)
                   .HasMaxLength(300);

            builder.Property(r => r.Source)
                   .HasMaxLength(100); // Modül adı vs.

            builder.Property(r => r.ReportData)
                   .HasColumnType("nvarchar(max)");

            builder.Property(r => r.Status)
                   .IsRequired();

            builder.Property(r => r.ReportType)
                   .IsRequired();

            builder.Property(r => r.ReportDate)
                   .IsRequired();

            builder.Property(r => r.ErrorMessage)
                   .HasMaxLength(500);

            builder.Property(r => r.IPAddress)
                   .HasMaxLength(45); // IPv6 destekler

            builder.Property(r => r.IsSystemGenerated)
                   .IsRequired();

            builder.Property(r => r.XmlFilePath)
                   .HasMaxLength(255);

            // 🔗 İlişkiler

            builder.HasOne(r => r.User)
                   .WithMany(u => u.ReportLogs)
                   .HasForeignKey(r => r.UserId)
                   .OnDelete(DeleteBehavior.SetNull); // Sistem logu ise user null olabilir

            builder.HasOne(r => r.Customer)
                   .WithMany(c => c.ReportLogs)
                   .HasForeignKey(r => r.CustomerId)
                   .OnDelete(DeleteBehavior.SetNull); // Müşteri ilişkisi opsiyonel

        }
    }
}
