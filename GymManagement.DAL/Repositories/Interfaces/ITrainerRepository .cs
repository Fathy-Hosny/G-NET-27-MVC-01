using System;
using System.Collections.Generic;
using System.Text;
using GymManagement.DAL.Models;

namespace GymManagement.DAL.Repositories.Interfaces
{
    public interface ITrainerRepository : IGenericRepository<Trainer>
    {
        Task<bool> IsEmailTakenAsync(string email, int? excludeId = null, CancellationToken ct = default);
        Task<bool> IsPhoneTakenAsync(string phone, int? excludeId = null, CancellationToken ct = default);
        Task<bool> HasScheduledSessionsAsync(int trainerId, CancellationToken ct = default);
    }
}
