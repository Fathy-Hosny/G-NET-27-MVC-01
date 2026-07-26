using System;
using System.Collections.Generic;
using System.Text;
using GymManagement.Models;

namespace GymManagement.DAL.Repositories.Interfaces
{
    public interface IPlanRepository : IGenericRepository<Plan>
    {
        Task<Plan?> GetPlanWithMembershipsAsync(int id, CancellationToken ct = default);
    }
}
