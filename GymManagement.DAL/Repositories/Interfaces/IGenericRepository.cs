using System;
using System.Collections.Generic;
using System.Text;
using GymManagement.DAL.Models;
using GymManagement.Models;

namespace GymManagement.DAL.Repositories.Interfaces
{
    public interface IGenericRepository <TEntity>  where TEntity : BaseEntity , new()
    {
        Task<IEnumerable<TEntity>> GetAllAsync(bool Tracking = false, CancellationToken ct = default);

        Task<TEntity?> GetByIdAsync(int id, CancellationToken ct = default);

        Task<int> AddAsync(TEntity entity, CancellationToken ct = default);
        Task<int> UpdateAsync(TEntity entity, CancellationToken ct = default);
        Task<int> DeleteAsync(TEntity entity , CancellationToken ct = default);
    }
}
