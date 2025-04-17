using Microsoft.EntityFrameworkCore;
using Project.Dal.ContextClasses;
using Project.Dal.Repositories.Abstracts;
using Project.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Project.Dal.Repositories.Concretes
{
    public abstract class BaseRepository<T> : IRepository<T> where T : class, IEntity
    {
        protected readonly MyContext _context;
        protected readonly DbSet<T> _dbSet;

        public BaseRepository(MyContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public  async Task<int> AddAndReturnIdAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();

            PropertyInfo? key = typeof(T).GetProperties().FirstOrDefault(p => p.Name == "Id");
            if (key != null)
            {
                object? idValue = key.GetValue(entity);
                if (idValue != null)
                    return Convert.ToInt32(idValue);
            }

            throw new Exception("Entity ID bulunamadı.");
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity); // ✅ Entity Framework asenkron metodu
            await _context.SaveChangesAsync();
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> exp)
        {
            return await _dbSet.AnyAsync(exp);
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> exp = null)
        {
            return exp == null ? await _dbSet.CountAsync() : await _dbSet.CountAsync(exp);
        }

        public async Task CreateAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> exp)
        {
            return await _dbSet.FirstOrDefaultAsync(exp);
        }

       

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate = null)
        {
            return predicate == null
                ? await _dbSet.ToListAsync()
                : await _dbSet.Where(predicate).ToListAsync();
        }

       

        public async Task<IEnumerable<T>> GetAllWithIncludeAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IQueryable<T>> include)
        {
            IQueryable<T> query = _dbSet;

            if (include != null)
                query = include(query);

            if (predicate != null)
                query = query.Where(predicate);

            return await query.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IQueryable<T>> include)
        {
            IQueryable<T> query = _context.Set<T>();

            if (include != null)
                query = include(query);

            return await query.FirstOrDefaultAsync(predicate);
        }

        public async Task<List<T>> GetPagedAsync(int pageIndex, int pageSize) //??
        {
            return await _dbSet.Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();
        }

        public IQueryable<T> OrderBy<TKey>(Expression<Func<T, TKey>> keySelector)
        {
            return _dbSet.OrderBy(keySelector);
        }

        public IQueryable<T> OrderByDescending<TKey>(Expression<Func<T, TKey>> keySelector)
        {
            return _dbSet.OrderByDescending(keySelector);
        }

        public async Task RemoveAsync(T entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public IQueryable<TResult> Select<TResult>(Expression<Func<T, TResult>> selector)
        {
            return _dbSet.Select(selector);
        }
        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> exp)
        {
            return _dbSet.Where(exp);
        }
    }
}
