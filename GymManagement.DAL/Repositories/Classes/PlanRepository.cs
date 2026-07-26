using System;
using System.Collections.Generic;
using System.Text;
using GymManagement.DAL.Repositories.Interfaces;
using GymManagement.DbContexts;
using GymManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.DAL.Repositories.Classes
{
    public class PlanRepository : GenericRepository<Plan>, IPlanRepository
    {
        private readonly GymDbcontext _context;

        public PlanRepository(GymDbcontext context) : base(context)
        {
            _context = context;
        }

        public async Task<Plan?> GetPlanWithMembershipsAsync(int id, CancellationToken ct = default)
        {
            return await _context.Plans.Include(p => p.Members).FirstOrDefaultAsync(p => p.Id == id, ct);
        }
    
}
}
