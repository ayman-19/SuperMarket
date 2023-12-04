using DB_Core.Models;
using DB_Core.Services;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using System.Text.Json;

namespace SuperMarket.Helpers
{
	public class OrderHelper
	{
		public static async Task AddItemInOrder(List<CartProduct> products, int orderId, IUnitOfWork _context, string token)
		{
			using (var httpclient = new HttpClient())
			{
				var url = "https://localhost:7255/api/OrderItems/AddItemsInOrder";
				List<OrderItem> items = new List<OrderItem>();
				foreach (CartProduct item in products)
				{
					if (!item.Out)
					{
						items.Add(new OrderItem { OrderId = orderId, ProductId = item.ProductId, Count = item.Count, TotalAmount = item.TotalAmount });

						_context.CartItems.CheckOut(item.CartId, item.ProductId);
					}
				}
				httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

				var response = await httpclient.PostAsJsonAsync(url, items.AsEnumerable());
			}
		}
	}
}
