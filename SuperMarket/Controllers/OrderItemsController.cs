using DB_Core.Models;
using DB_Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using SuperMarket.Dtos;

namespace SuperMarket.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	//[Authorize]
	public class OrderItemsController : ControllerBase
	{
		private readonly IUnitOfWork _context;

		public OrderItemsController(IUnitOfWork context)
		{
			_context = context;
		}
		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			return Ok(await _context.OrderItems.GetAllAsync(includes: new string[] { "Order", "Product" }));
		}

		[HttpGet("{oredrId}/{productId}")]
		public async Task<IActionResult> Get(int oredrId, int productId)
		{
			var orderitem = await _context.OrderItems.GetAsync(x => x.ProductId == productId && x.OrderId == oredrId, new string[] { "Order", "Product" });
			if (orderitem != null)
				return Ok(orderitem);
			return BadRequest("InValid!");
		}
		[HttpPost("AddItemInOrder")]
		public async Task<IActionResult> Add(ItemInOrderDto dto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);
			var product = await _context.Products.GetAsync(x => x.Id == dto.ProductId);
			if (dto.Count > product.Quantity)
				return BadRequest($"Valid {product.Quantity} Only!");
			await _context.OrderItems.Add(new OrderItem { Count = dto.Count, OrderId = dto.OrderId, ProductId = dto.ProductId, TotalAmount = dto.Count * product.price });
			product.Quantity -= dto.Count;
			_context.Products.Update(product);
			_context.SaveChanges();
			return Ok(dto);
		}
		[HttpPost("AddItemsInOrder")]
		public async Task<IActionResult> Add(IEnumerable<OrderItem> items)
		{
			if (items != null)
			{
				await _context.OrderItems.AddRange(items);
				_context.SaveChanges();
			}
			return Ok();
		}
		[HttpDelete("DeleteItem")]
		public async Task<IActionResult> Delete(DeleteItemInOrderDto dto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);
			var orderitem = await _context.OrderItems.GetAsync(x => x.ProductId == dto.ProductId && x.OrderId == dto.OrderId, new string[] { "Order", "Product" });
			if (orderitem != null)
			{
				_context.OrderItems.Delete(orderitem);
				_context.SaveChanges();
				return Ok(orderitem);
			}
			return BadRequest("InValid!");
		}
		[HttpPut("Update")]
		public async Task<IActionResult> Update(ItemInOrderDto dto) 
		{
			var orderitem = await _context.OrderItems.GetAsync(x => x.ProductId == dto.ProductId && x.OrderId == dto.OrderId, new string[] { "Order", "Product" });
			if (orderitem != null)
			{
				if (dto.Count > orderitem.Product!.Quantity)
					return BadRequest($"Valid {orderitem.Product!.Quantity} Only!");
				if (dto.Count < orderitem.Count)
				{
					orderitem.Product!.Quantity += (orderitem.Count - dto.Count);
				}
				orderitem.Count = dto.Count;
				orderitem.TotalAmount = dto.Count * orderitem.Product!.price;
				_context.OrderItems.Update(orderitem);
				_context.SaveChanges();
				return Ok(dto);
			}
			return BadRequest("InValid");
		}
	}
}
