using System;
using System.Collections.Generic;
using System.Text;
using GymManagement.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagement.DAL.Configurations
{
    public class SessionConfiguration : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.ToTable(tb=> {


                tb.HasCheckConstraint("SessionCapacityCheck", "Capacity Between 1 And 25");
                tb.HasCheckConstraint("SessionDateCheck", "EndDate > StartDate");




            });   
        }
    }
}
