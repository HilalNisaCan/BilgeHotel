using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Project.Conf.Options
{
    public class CustomerConfiguration:BaseConfiguration<Customer>
    {
        public override void Configure(EntityTypeBuilder<Customer> builder)
        {

            // 🔹 User ile 1:1 ilişki
            builder.HasOne(c => c.User)
        .WithOne(u => u.Customer)
        .HasForeignKey<Customer>(c => c.UserId)
        .OnDelete(DeleteBehavior.Restrict);  // Kullanıcı silinirse, müşterisi de silinsin

            // 🔹 IdentityNumber için zorunluluk
            builder.Property(c => c.IdentityNumber)
                   .IsRequired()
                   .HasMaxLength(11); // T.C. kimlik numarası 11 haneli olmalı

            // 🔹 BillingDetails zorunlu
            builder.Property(c => c.BillingDetails)
                   .IsRequired()
                   .HasMaxLength(250);

            // 🔹 LoyaltyPoints varsayılan değer
            builder.Property(c => c.LoyaltyPoints)
           .HasColumnType("decimal(10, 2)");






        }
    }
}
