using System.Reflection;
using GymManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.DbContexts
{
    public class GymDbcontext : DbContext
    {
        public GymDbcontext(DbContextOptions<GymDbcontext> options) : base(options)
        {
        }   
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);

        }
      
       public DbSet<Plan> Plans { get; set; }
    }
}
