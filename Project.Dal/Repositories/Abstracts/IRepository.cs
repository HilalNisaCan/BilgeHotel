using Project.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Project.Dal.Repositories.Abstracts
{
    public interface IRepository<T> where T : class, IEntity
    {
        // Queries
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate = null); // ✅ Parametreyi destekler hale getirdik 
        Task AddAsync(T entity);
        Task<T> GetByIdAsync(int id);
        IQueryable<T> Where(Expression<Func<T, bool>> exp);
        Task<int> CountAsync(Expression<Func<T, bool>> exp = null);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> exp);
        Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IQueryable<T>> include);
        Task<IEnumerable<T>> GetAllWithIncludeAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IQueryable<T>> include);
        Task<int> AddAndReturnIdAsync(T entity);



        Task<bool> AnyAsync(Expression<Func<T, bool>> exp);
        Task<List<T>> GetPagedAsync(int pageIndex, int pageSize);

        // `IQueryable` Gelişmiş Sorgular
        IQueryable<TResult> Select<TResult>(Expression<Func<T, TResult>> selector); // Belirli alanları seçme
        IQueryable<T> OrderBy<TKey>(Expression<Func<T, TKey>> keySelector); // Artan sıralama
        IQueryable<T> OrderByDescending<TKey>(Expression<Func<T, TKey>> keySelector); // Azalan sıralama

        // Commands
        Task CreateAsync(T entity);
        Task RemoveAsync(T entity);
        Task UpdateAsync(T entity);
        
    }
}
