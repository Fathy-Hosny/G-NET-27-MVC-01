using System.Reflection;
using GymManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.DbContexts
{
    public class GymDbcontext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=GymDb;Trusted_Connection=True; TrustServerCertificate=true;");
        }
       public DbSet<Plan> Plans { get; set; }
    }
}
