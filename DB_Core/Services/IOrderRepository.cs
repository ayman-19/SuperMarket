using DB_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DB_Core.Services
{
    public interface IOrderRepository : IRepository<Order>
    {
        public Task OrderState(int orderId, OrderState state, double total);

		Task<int> Count(Expression<Func<Order, bool>> expression = null!);
    }
}
