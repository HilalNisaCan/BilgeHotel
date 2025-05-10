using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Conf.Options
{
    public class AppRoleConfiguration : IEntityTypeConfiguration<AppRole>
    {
        public void Configure(EntityTypeBuilder<AppRole> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(x => x.NormalizedName)
                   .HasMaxLength(50);

            builder.Property(x => x.Description)
                   .HasMaxLength(150);

         

            builder.ToTable("AspNetRoles"); // Identity default tablosunu koruyalım
        }
    }
}
