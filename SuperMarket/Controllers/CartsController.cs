using DB_Core.Models;
using DB_Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SuperMarket.Dtos;
using SuperMarket.Helpers;

namespace SuperMarket.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	//[Authorize]
	public class CartsController : ControllerBase
	{
		private readonly IUnitOfWork _context;

		public CartsController(IUnitOfWork context)
		{
			_context = context;
		}
		[HttpGet]
		public async Task<IActionResult> GetAll()
		{ 
			return Ok(await _context.Carts.GetAllAsync(includes: new string[] { "CartProducts", "User" }));
		}
		[HttpGet("{id}")]
		public async Task<IActionResult> Get(int id)
		{
			var cart = await _context.Carts.GetAsync(x => x.Id == id, new string[] { "CartProducts", "User" });
			if(cart is not null)
				return Ok(cart);
			return BadRequest("InValid");
		}
		[HttpPost("AddCart")]
		public async Task<IActionResult> Add(AddCartDto dto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);
			await _context.Carts.Add(new Cart { UserId = dto.UserId });
			_context.SaveChanges();
			return Ok(dto);
		}
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			var cart = await _context.Carts.GetAsync(x => x.Id == id, new string[] { "CartProducts", "User" });
			if (cart is not null)
			{
				_context.Carts.Delete(cart);
				_context.SaveChanges();
				return Ok("Removed");
			}
			return BadRequest("InValid");
		}
		[HttpPut("{id}")]
		public async Task<IActionResult> Update(int id)
		{
			var cart = await _context.Carts.GetAsync(x => x.Id == id, new string[] { "CartProducts", "User" });
			if ( cart is not null)
			{
				var prod = cart.CartProducts ?? new List<CartProduct>();
				cart.Total = prod.Sum(x => x.TotalAmount);
				_context.Carts.Update(cart);
				_context.SaveChanges();
				return Ok(cart);
			}
			return BadRequest("InValid");
		}
		[HttpPost("Progress")]

		public async Task<IActionResult> Progress(ProgDto dto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);
			var cart = await _context.Carts.GetAsync(x => x.UserId == dto.UserId, new string[] { "CartProducts", "User" });
			
			if (cart is not null)
			{
				if (cart.CartProducts is not null)
					await OrderHelper.AddItemInOrder(cart.CartProducts, dto.OrderId, _context, (await _context.Tokens.GetAsync(x => x.UserId == cart.UserId)).TokenValue);
				return Ok("Compelted");
			}
			return BadRequest("InValid");
		}
		[HttpGet("CheckOut/{orderId}")]
		public async Task<IActionResult> CheckOut(int orderId)
		{
			var order = await _context.Orders.GetAsync(x => x.Id == orderId, new string[] { "OrderItems", "User" });
			if (order is null)
				return BadRequest("InValid Order!");
			var products = order.OrderItems;
			if (products is null)
				return BadRequest("InValid Products!");
			var prods = await CheckOutHelper.CheckOutOrder(products, orderId, _context);
			return Ok(new OrderDetailsDto
			{
				Date = order.Date,
				Products = prods,
				Total = CheckOutHelper.Total,
			});
		}
	}
}
