using System;
using System.Collections.Generic;
using System.Text;
using GymManagement.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagement.DAL.Configurations
{
    public class TrainerConfiguration : GymUserConfigurations<Trainer>, IEntityTypeConfiguration<Trainer>
    {
       
            public void Configure(EntityTypeBuilder<Trainer> builder)
            {
                builder.Property(M => M.CreatedAt)
                    .HasColumnName("HireDate")
                    .HasDefaultValueSql("GETDATE()");
                base.Configure(builder);
            }
        
    }
}
