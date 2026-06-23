using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using GymManagement.DAL.Models;
using GymManagement.Models;

namespace GymManagement.DAL.Repositories.Interfaces
{
    public interface IGenericRepository <TEntity>  where TEntity : BaseEntity 
    {
        Task<IEnumerable<TEntity>> GetAllAsync(bool Tracking = false, CancellationToken ct = default);

        Task<TEntity?> GetByIdAsync(int id, CancellationToken ct = default);

      void Add(TEntity entity );
      void Update(TEntity entit);
        void Delete(TEntity entit);

        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default);
        Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default);
    }
}
