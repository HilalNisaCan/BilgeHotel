using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Entities.Interfaces;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Conf.Options
{
    public class RoomImageConfiguration:BaseConfiguration<RoomImage>
    {
        public override void Configure(EntityTypeBuilder<RoomImage> builder)
        {
            builder.HasKey(ri => ri.Id);

            builder.Property(ri => ri.ImagePath)
                   .IsRequired()
                   .HasMaxLength(255);

            builder.Property(ri => ri.IsMain)
                   .IsRequired();

            builder.HasOne(ri => ri.Room)
                   .WithMany(r => r.RoomImages)
                   .HasForeignKey(ri => ri.RoomId)
                   .OnDelete(DeleteBehavior.Cascade); // Oda silinirse görseller de silinsin


        }
    }
}
