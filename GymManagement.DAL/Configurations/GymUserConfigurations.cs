using System;
using System.Collections.Generic;
using System.Text;
using GymManagement.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagement.DAL.Configurations
{
    public class GymUserConfigurations<T> : IEntityTypeConfiguration<T> where T : GymUser
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(u => u.Name)
                .HasColumnType("varchar")
                .HasMaxLength(50);

            builder.Property(u => u.Email)
               .HasColumnType("varchar")
               .HasMaxLength(100);

            builder.Property(u => u.Phone)
               .HasColumnType("varchar")
               .HasMaxLength(11);

            builder.HasIndex(u => u.Phone).IsUnique();
            builder.HasIndex(u => u.Email).IsUnique();

            builder.OwnsOne(u => u.Address, address => {
           address.Property(A => A.Street)
           .HasColumnName("Street")
           .HasColumnType("varchar(30)");

            address.Property(A => A.City)
            .HasColumnName("City")
            .HasColumnType("varchar(30)");

             address.Property(A => A.BuildingNumber)
                  .HasColumnName("BuildingNumber");
                  

            } );

            builder.ToTable(tb =>
            {
                tb.HasCheckConstraint("EmailChek", "Email Like '_%@_%._%'");
                tb.HasCheckConstraint("PhoneChek", "Phone Like '010%' or Phone Like '011%' or Phone Like '012%' or Phone Like '015%'");

            });

        }
    }
}
