using System.Linq.Expressions;
namespace DB_Core.Services
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetAsync(Expression<Func<T, bool>> predicate, string[] includes = default!);
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate = default!, string[] includes = default!);
        void Update(T entity);
        void Delete(T entity);
        Task Add(T entity);
        Task AddRange(IEnumerable<T> entities);
		void DeleteRange(IEnumerable<T> entities);
        Task<bool> IsAnyExist(Expression<Func<T, bool>> predicate);
    }
}
