using DB_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DB_Core.Services
{
	public interface ICartItem : IRepository<CartProduct>
	{
		public void CheckOut(int cartId, int itemId);
		Task<int> Count(Expression<Func<CartProduct, bool>> expression = null!);
	}
}
