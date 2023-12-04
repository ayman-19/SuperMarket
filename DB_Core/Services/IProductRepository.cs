using DB_Core.Models;
using System.Linq.Expressions;

namespace DB_Core.Services
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<int> Count(Expression<Func<Product, bool>> expression = null!);
    }
}
