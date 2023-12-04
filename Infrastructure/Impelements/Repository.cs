using DB_Core.Services;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Impelements
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly SuperMarketContext _context;
        protected readonly DbSet<T> _entities;

        public Repository(SuperMarketContext context)
        {
            _context = context;
            _entities = _context.Set<T>();
        }
        public async Task Add(T entity)
        {
            await _entities.AddAsync(entity);
        }

        public void Delete(T entity)
        {
            _entities.Remove(entity);
        }
        public async Task<bool> IsAnyExist(Expression<Func<T, bool>> predicate)
        {
            return await _entities.AnyAsync(predicate);
        }

        public void Update(T entity)
        {
            //if (_entities.Entry(entity).State == EntityState.Detached)
            //    _entities.Attach(entity);
            //_entities.Entry(entity).State = EntityState.Modified;
            _entities.Update(entity);
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate = null!, string[] includes = null!)
        {
            var src = _entities.AsQueryable();
            if (predicate != null)
            {
                if (includes != null)
                    foreach (var include in includes)
                        src = src.Where(predicate).Include(include);
                else
                    src = src.Where(predicate);
            }
            else
            {
                if (includes != null)
                    foreach (var include in includes)
                        src = src.Include(include);
            }
            return await src.ToListAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> predicate, string[] includes = null!)
        {
			var src = _entities.AsQueryable();
			if (includes != null)
				foreach (var include in includes)
					src = src.Where(predicate).Include(include);
            return await src.FirstOrDefaultAsync(predicate);
        }

		public void DeleteRange(IEnumerable<T> entities)
		{
			_entities.RemoveRange(entities);
		}

		public async Task AddRange(IEnumerable<T> entities)
		{
			_context.AddRangeAsync(entities);
		}
	}
}
