using DB_Core.Models;
using DB_Core.Services;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Impelements
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(SuperMarketContext context) : base(context)
        {
        }

        public async Task<int> Count(Expression<Func<Category, bool>> expression = null!)
        {
            if (expression != null)
                return await _entities.CountAsync(expression);
            return _entities.Count();
        }
    }
}
