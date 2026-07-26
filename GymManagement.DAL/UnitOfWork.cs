using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Classes;
using GymManagement.DAL.Repositories.Interfaces;
using GymManagement.DbContexts;

namespace GymManagement.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GymDbcontext _context;
        private readonly Dictionary<string, object> _repositories = [];
        private readonly ISessionRepository _sessionRepository;
        private readonly IPlanRepository _planRepository;
        private readonly ITrainerRepository _trainerRepository;
        public UnitOfWork(GymDbcontext context, ISessionRepository sessionRepository , IPlanRepository planRepository, ITrainerRepository trainerRepository)
        {
            _context = context;
            _sessionRepository = sessionRepository;
            _planRepository = planRepository;
            _trainerRepository = trainerRepository;
        }

        public ISessionRepository SessionRepository => _sessionRepository;
        public IPlanRepository PlanRepository => _planRepository;
        public ITrainerRepository TrainerRepository => _trainerRepository;
        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity
        {
            var typeName = typeof(TEntity).Name;

            if (_repositories.TryGetValue(typeName, out object? value))
                return value as IGenericRepository<TEntity>;

            var repo = new GenericRepository<TEntity>(_context);
            _repositories.Add(typeName, repo);
            return repo;
        }

        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
       => await _context.SaveChangesAsync(ct);
    }
}
