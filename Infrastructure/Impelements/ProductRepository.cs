using DB_Core.Models;
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
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(SuperMarketContext context) : base(context)
        {
        }

        public async Task<int> Count(Expression<Func<Product, bool>> expression = null!)
        {
            if (expression != null)
                return await _entities.CountAsync(expression);
            return _entities.Count();
        }
    }
}
