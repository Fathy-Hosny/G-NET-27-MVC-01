using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;
using GymManagement.DbContexts;
using GymManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.DAL.Repositories.Classes
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
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

        public void Add(TEntity entity)
        {
            _dbSet.Add(entity);
        }
        public void Update(TEntity entity)
        {
            _dbSet.Update(entity);


           
        }

        public void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);

        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default)
        {
            return await _context.Set<TEntity>().AnyAsync(predicate, ct);
        }

        public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default)
        {
            return await _context.Set<TEntity>().FirstOrDefaultAsync(predicate, ct);      
            
        }
    }
}
