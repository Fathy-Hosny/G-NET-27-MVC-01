using System;
using System.Collections.Generic;
using System.Text;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;
using GymManagement.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.DAL.Repositories.Classes
{
    public class SessionRepository : GenericRepository<Session>, ISessionRepository
    {
        private readonly GymDbcontext _context;

        public SessionRepository(GymDbcontext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Session>> GetAllSessionWithTrainerAndCategoryAsync(CancellationToken ct = default)
        => await _context.Sessions.AsNoTracking().Include(s => s.Trainer).Include(s => s.Category).ToListAsync(ct);
        

        public async Task<int> GetCountOfBookedSlotsAsync(int sessionId, CancellationToken ct = default)
      => await _context.Bookings.AsNoTracking().CountAsync(b => b.SessionId == sessionId, ct);
        

        public async Task<Session?> GetSessionWithTrainerAndCategoryByIdAsync(int sessionId, CancellationToken ct = default)
        => await _context.Sessions.AsNoTracking().Include(s => s.Trainer).Include(s => s.Category).FirstOrDefaultAsync(s => s.Id == sessionId, ct);
        
    }
}
