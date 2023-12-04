using DB_Core.Models;
using System.Linq.Expressions;

namespace DB_Core.Services
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<int> Count(Expression<Func<Category, bool>> expression = null!);
    }
}
