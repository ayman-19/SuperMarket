using DB_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DB_Core.Services
{
    public interface ICartRepository : IRepository<Cart>
    {
        Task<int> Count(Expression<Func<Cart, bool>> expression = null!);
    }
}
