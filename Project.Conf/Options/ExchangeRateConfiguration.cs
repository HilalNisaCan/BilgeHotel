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
    public class ExchangeRateConfiguration:BaseConfiguration<ExchangeRate>
    {
        public override void Configure(EntityTypeBuilder<ExchangeRate> builder)
        {
            builder.HasKey(e => e.Id); // 🔑 Anahtar

            // 💱 Döviz kodu (örn: USD)
            builder.Property(e => e.CurrencyCode)
                   .HasMaxLength(10)
                   .IsRequired();

            // 💹 Kur oranı
            builder.Property(e => e.Rate)
                   .HasColumnType("decimal(18,6)") // yüksek hassasiyet
                   .IsRequired();

            builder.Property(e => e.Date)
                   .IsRequired();

            builder.Property(e => e.FromCurrency)
                   .IsRequired();

            builder.Property(e => e.ToCurrency)
                   .IsRequired();

        }
    }
}
