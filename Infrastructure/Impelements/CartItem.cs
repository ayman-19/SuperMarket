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
	public class CartItem : Repository<CartProduct>, ICartItem
	{
		public CartItem(SuperMarketContext context) : base(context)
		{
		}
		public void CheckOut(int cartId, int itemId)
		{
			_entities.Where(x => x.CartId == cartId && x.ProductId == itemId).ExecuteUpdate(c => c.SetProperty(x => x.Out, true));
		}

		public async Task<int> Count(Expression<Func<CartProduct, bool>> expression = null!)
		{
			if (expression != null)
				return await _entities.CountAsync(expression);
			return _entities.Count();
		}
	}
}
