using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Entities.Enums;

namespace Project.Conf.Options
{
    public class RoomTypePriceConfiguration : IEntityTypeConfiguration<RoomTypePrice>
    {
        public void Configure(EntityTypeBuilder<RoomTypePrice> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.RoomType)
                   .IsRequired();

            builder.Property(x => x.PricePerNight)
                   .HasColumnType("decimal(10,2)");

            
        }
    }
}

