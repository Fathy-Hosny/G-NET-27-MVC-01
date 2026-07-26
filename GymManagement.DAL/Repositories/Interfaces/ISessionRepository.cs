using System;
using System.Collections.Generic;
using System.Text;
using GymManagement.DAL.Models;

namespace GymManagement.DAL.Repositories.Interfaces
{
    public interface ISessionRepository : IGenericRepository<Session>
    {
        Task<IEnumerable<Session>> GetAllSessionWithTrainerAndCategoryAsync(CancellationToken ct = default);
        Task<int> GetCountOfBookedSlotsAsync(int sessionId, CancellationToken ct = default);
        Task<Session?> GetSessionWithTrainerAndCategoryByIdAsync(int sessionId, CancellationToken ct = default);
    }
}
