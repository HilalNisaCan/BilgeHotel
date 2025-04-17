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
    public class CampaignConfiguration:BaseConfiguration<Campaign>
    {
        public override void Configure(EntityTypeBuilder<Campaign> builder)
        {

            builder.HasKey(c => c.Id); // 🔑 Anahtar

            // 📢 Kampanya bilgileri
            builder.Property(c => c.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(c => c.ProductImagePath)
                   .HasMaxLength(255); // Kampanya görseli

            builder.Property(c => c.DiscountPercentage)
                   .HasColumnType("decimal(5,2)")
                   .IsRequired();

            builder.Property(c => c.ValidityDays)
                   .IsRequired();

            builder.Property(c => c.Status)
                   .IsRequired();

            builder.Property(c => c.CampaignType)
                   .IsRequired();

            builder.Property(c => c.Package)
                   .IsRequired();

            builder.Property(c => c.StartDate)
                   .IsRequired();

            builder.Property(c => c.EndDate)
                   .IsRequired();

            builder.Property(c => c.IsActive)
                   .IsRequired();

            // 🔗 İlişkiler

            builder.HasMany(c => c.Reservations)
                   .WithOne(r => r.Campaign)
                   .HasForeignKey(r => r.CampaignId)
                   .OnDelete(DeleteBehavior.SetNull); // Kampanya silinse bile rezervasyon kalsın

            builder.HasMany(c => c.EarlyReservationDiscounts)
                   .WithOne(d => d.Campaign)
                   .HasForeignKey(d => d.CampaignId)
                   .OnDelete(DeleteBehavior.SetNull); // Aynı şekilde birebir ilişki değilse null olabilir


        }
    }
}
