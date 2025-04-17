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
    public class ComplaintLogConfiguration : BaseConfiguration<ComplaintLog>
    {
        public override void Configure(EntityTypeBuilder<ComplaintLog> builder)
        {
            builder.HasKey(c => c.Id); // 🔑 Primary Key

         

            builder.Property(c => c.Description)
                   .HasMaxLength(500); // Şikayet açıklaması

            builder.Property(c => c.Status)
                   .IsRequired(); // Enum: Bekliyor, İnceleniyor, Yanıtlandı vs.

            builder.Property(c => c.SubmittedDate)
                   .IsRequired(); // Ne zaman gönderildi?

            builder.Property(c => c.Response)
                   .HasMaxLength(500); // Verilen cevap

            builder.Property(c => c.IsResolved)
                   .IsRequired(); // Çözüldü mü?

            // 🔗 İlişkiler

            builder.HasOne(c => c.Customer)
                   .WithMany(cu => cu.ComplaintLogs)
                   .HasForeignKey(c => c.CustomerId)
                   .OnDelete(DeleteBehavior.Cascade); // Müşteri silinirse şikayetleri de silinir
        }
    }
}
