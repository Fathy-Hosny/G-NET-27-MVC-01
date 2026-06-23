using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;

namespace GymManagement.DAL
{
    public interface IUnitOfWork
    {
        IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity;

        Task<int> SaveChangesAsync(CancellationToken ct = default);
        public ISessionRepository SessionRepository { get; }
        public IPlanRepository PlanRepository { get; }
    }
}
