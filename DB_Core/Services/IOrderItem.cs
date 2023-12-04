using DB_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DB_Core.Services
{
	public interface IOrderItem: IRepository<OrderItem>
	{
		public Task CheckOut(int orderId, int productId);
		Task<int> Count(Expression<Func<OrderItem, bool>> expression = null!);
	}
}
