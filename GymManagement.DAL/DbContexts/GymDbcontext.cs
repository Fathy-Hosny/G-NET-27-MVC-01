using System.Reflection;
using GymManagement.DAL.Models;
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
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<HealthRecord> HealthRecords { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<MemberShip> MemberShips { get; set; }








    }
}
