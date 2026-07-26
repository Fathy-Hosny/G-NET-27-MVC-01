using System;
using System.Collections.Generic;
using System.Text;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;
using GymManagement.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.DAL.Repositories.Classes
{
    public class TrainerRepository : GenericRepository<Trainer>, ITrainerRepository
    {
        private readonly GymDbcontext _context;

        public TrainerRepository(GymDbcontext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> IsEmailTakenAsync(string email, int? excludeId = null, CancellationToken ct = default)
        {
            return await _context.Trainers
                .AnyAsync(t => t.Email == email && (!excludeId.HasValue || t.Id != excludeId.Value), ct);
        }

        public async Task<bool> IsPhoneTakenAsync(string phone, int? excludeId = null, CancellationToken ct = default)
        {
            return await _context.Trainers
                .AnyAsync(t => t.Phone == phone && (!excludeId.HasValue || t.Id != excludeId.Value), ct);
        }

        public async Task<bool> HasScheduledSessionsAsync(int trainerId, CancellationToken ct = default)
        {
         
            return await _context.Sessions
                .AnyAsync(s => s.TrainerId == trainerId && s.EndDate >= DateTime.Now, ct);
        }
    }
}
