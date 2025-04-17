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

            // 🔗 İlişkiler

            builder.HasOne(r => r.User) // Kullanıcı ile ilişki
                   .WithMany(u => u.Reviews)
                   .HasForeignKey(r => r.UserId)
                   .OnDelete(DeleteBehavior.Cascade); // Kullanıcı silinirse yorum da silinsin

            builder.HasOne(r => r.Reservation) // Rezervasyon ile ilişki
                   .WithMany(rz => rz.Reviews)
                   .HasForeignKey(r => r.ReservationId)
                   .OnDelete(DeleteBehavior.SetNull); // Rezervasyon silinirse yorum kalabilir

            builder.HasOne(r => r.Room) // Oda ile ilişki
                   .WithMany(rm => rm.Reviews)
                   .HasForeignKey(r => r.RoomId)
                   .OnDelete(DeleteBehavior.SetNull); // Oda silinirse yorum kalabilir
        }
    }
}
