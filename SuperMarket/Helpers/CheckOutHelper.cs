using DB_Core.Models;
using DB_Core.Services;
using SuperMarket.Dtos;

namespace SuperMarket.Helpers
{
	public class CheckOutHelper
	{
		public static double Total = 0;
		public static async Task<List<ProductDetailsDto>> CheckOutOrder(List<OrderItem> Items, int orderId, IUnitOfWork _context)
		{
			var ListProduct = new List<ProductDetailsDto>();
			foreach (var item in Items)
			{
				if (!item.Out)
				{
					var product = await _context.Products.GetAsync(x => x.Id == item.ProductId);
					ListProduct.Add(new ProductDetailsDto()
					{
						ProductName = product.Name,
						Amount = item.Count,
						TotalPrice = item.Count * product.price,
					});
					await _context.OrderItems.CheckOut(item.OrderId, item.ProductId);
				}
			}
			Total = ListProduct.Sum(x => x.TotalPrice);
			await _context.Orders.OrderState(orderId, OrderState.End, Total);
			return ListProduct;
		}
	}
}
