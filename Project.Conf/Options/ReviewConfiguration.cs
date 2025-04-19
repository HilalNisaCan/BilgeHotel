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
    public class ReviewConfiguration:BaseConfiguration<Review>
    {
        public override void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.HasKey(r => r.Id); // 🔑 Primary key

            builder.Property(r => r.Rating)
                   .IsRequired()  // ⭐️ Puan zorunlu
                   .HasDefaultValue(1); // 1-5 arasında bir puan olmalı, default 1

            builder.Property(r => r.Comment)
                   .HasMaxLength(1000); // 💬 Yorum uzunluğu

            builder.Property(r => r.CommentDate)
                   .IsRequired(); // 🗓️ Yorum tarihi

            builder.Property(r => r.IsApproved)
                   .IsRequired(); // Onaylı mı? (moderasyon)

            builder.Property(r => r.IsAnonymous)
                   .IsRequired(); // Yorum anonim mi?

            builder.Property(r => r.RoomType)
           .HasConversion<int>() // enum -> int
           .IsRequired();

            // 🔗 İlişkiler

            builder.HasOne(r => r.User) // Kullanıcı ile ilişki
                   .WithMany(u => u.Reviews)
                   .HasForeignKey(r => r.UserId)
                   .OnDelete(DeleteBehavior.Cascade); // Kullanıcı silinirse yorum da silinsin

            

          
        }
    }
}
