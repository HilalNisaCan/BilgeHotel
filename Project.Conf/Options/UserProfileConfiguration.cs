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
    public class UserProfileConfiguration:BaseConfiguration<UserProfile>
    {
        public override void Configure(EntityTypeBuilder<UserProfile> builder)
        {
            builder.HasKey(p => p.Id); // 🔑 Primary key

            // 📇 Kişisel bilgiler
            builder.Property(p => p.FirstName)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(p => p.LastName)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(p => p.Address)
                   .HasMaxLength(250);

            builder.Property(p => p.BirthDate)
                   .IsRequired();


            builder.Property(p => p.ProfileImagePath)
                   .HasMaxLength(255); // Profil görseli


            // 🔗 İlişki (User ile birebir)
            builder.HasOne(p => p.User)
                   .WithOne(u => u.UserProfile)
                   .HasForeignKey<UserProfile>(p => p.UserId)
                   .OnDelete(DeleteBehavior.Cascade); // Kullanıcı silinirse profil de silinsin

        }
    }
}
