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
	public class ProductInCartController : ControllerBase
	{
		private readonly IUnitOfWork _context;

		public ProductInCartController(IUnitOfWork context)
		{
			_context = context;
		}
		[HttpGet("GatAll/{cartId}")]
		//[Authorize(Roles = "Admin")]
		public async Task<IActionResult> GetAll(int cartId)
		{
			return Ok(await _context.CartItems.GetAllAsync(x => x.CartId == cartId, includes: new string[] { "Product", "Cart" }));
		}
		[HttpGet("{id}")]
		public async Task<IActionResult> Get(int id)
		{
			var cartItem = await _context.CartItems.GetAsync(x => x.CartId == id, new string[] { "Product", "Cart" });
			if (cartItem is null)
				return NotFound();
			return Ok(cartItem);
		}
		[HttpPost("Add-ProductToCart")]
		public async Task<IActionResult> Post(AddCartItem dto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);
			var item = await _context.CartItems.GetAsync(x => x.ProductId == dto.ProductId && x.CartId == dto.CartId);

			var product = await _context.Products.GetAsync(x => x.Id == dto.ProductId);

			var cart = await _context.Carts.GetAsync(x => x.Id == dto.CartId);

			if (product is null || cart is null)
				return BadRequest("Cart Or Product There isn't any!");


			if (dto.Count > product.Quantity)
				return BadRequest($"There are only {product.Quantity}");

			var newitem = new CartProduct();

			if (item is null)
			{
				newitem.ProductId = dto.ProductId;
				newitem.CartId = dto.CartId;
				newitem.Count = dto.Count;
				newitem.TotalAmount = product.price * dto.Count;
				await _context.CartItems.Add(newitem);
			}
			else
			{
				if (!item.Out)
				{
					item.Count += dto.Count;
					item.TotalAmount += (product.price * dto.Count);
					_context.CartItems.Update(item);
				}
				else
				{
					item.Out = false;
					item.Count = dto.Count;
					item.TotalAmount = product.price * dto.Count;
					_context.CartItems.Update(item);
				}
			}
			product.Quantity -= dto.Count;
			_context.Products.Update(product);

			_context.SaveChanges();
			return Ok("Added");
		}
		[HttpDelete("Delete-Item")]
		public async Task<IActionResult> Delete(DeleteIItemInCartDto dto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var item = await _context.CartItems.GetAsync(x => x.CartId == dto.CartId && x.ProductId == dto.ProductId);
			if (item is null)
				return BadRequest("Product There isn't Cart!");

			_context.CartItems.Delete(item);
			_context.SaveChanges();
			return Ok("Delete Completed!");
		}
	}
}
