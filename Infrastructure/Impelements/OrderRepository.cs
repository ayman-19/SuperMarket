using DB_Core.Models;
using DB_Core.Services;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Impelements
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(SuperMarketContext context) : base(context)
        {
        }
        public async Task OrderState(int orderId, OrderState state, double total)
        {
            await _entities.Where(x => x.Id == orderId).ExecuteUpdateAsync(x => x.SetProperty(x => x.State, state).SetProperty(x => x.TotalPrice, total));
		}
        public async Task<int> Count(Expression<Func<Order, bool>> expression = null!)
        {
            if (expression != null)
                return await _entities.CountAsync(expression);
            return _entities.Count();
        }
	}
}
