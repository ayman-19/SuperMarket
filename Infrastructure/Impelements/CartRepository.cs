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
    public class CartRepository : Repository<Cart>, ICartRepository
    {
        public CartRepository(SuperMarketContext context) : base(context)
        {
        }

        public async Task<int> Count(Expression<Func<Cart, bool>> expression = null!)
        {
            if (expression != null)
                return await _entities.CountAsync(expression);
            return _entities.Count();
        }
    }
}
