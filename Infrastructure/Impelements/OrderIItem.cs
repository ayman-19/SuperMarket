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
	internal class OrderIItem : Repository<OrderItem>, IOrderItem
	{
		public OrderIItem(SuperMarketContext context) : base(context)
		{
		}
		public async Task CheckOut(int orderId, int productId)
		{
			await _entities.Where(x => x.ProductId == productId && x.OrderId == orderId)
				.ExecuteUpdateAsync(x => x.SetProperty(x => x.Out, true));
		}
		public async Task<int> Count(Expression<Func<OrderItem, bool>> expression = null!)
		{
			if (expression != null)
				return await _entities.CountAsync(expression);
			return _entities.Count();
		}
	}
}
