using System;
using System.Collections.Generic;
using System.Text;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;
using GymManagement.DbContexts;
using GymManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.DAL.Repositories.Classes
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity, new()
    {
        private readonly GymDbcontext _context;

      private readonly DbSet<TEntity> _dbSet;

        public GenericRepository(GymDbcontext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }




        public async Task<IEnumerable<TEntity>> GetAllAsync(bool Tracking = false, CancellationToken ct = default)
       => Tracking ? await _dbSet.ToListAsync(ct) : await _context.Set<TEntity>().AsNoTracking().ToListAsync(ct);


        public async Task<TEntity?> GetByIdAsync(int id, CancellationToken ct = default)
        => await _dbSet.FirstOrDefaultAsync(e => e.Id == id, ct);

        public async Task<int> AddAsync(TEntity entity, CancellationToken ct = default)
        {
            await _dbSet.AddAsync(entity, ct);
            return await _context.SaveChangesAsync(ct);
        }
        public async Task<int> UpdateAsync(TEntity entity, CancellationToken ct = default)
        {
            _dbSet.Update(entity);

            return await _context.SaveChangesAsync(ct);
        }

        public async Task<int> DeleteAsync(TEntity entity, CancellationToken ct = default)
        {
            _dbSet.Remove(entity);

            return await _context.SaveChangesAsync(ct);
        }





    }
}
