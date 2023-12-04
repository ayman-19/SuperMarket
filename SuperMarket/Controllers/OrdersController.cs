using DB_Core.Models;
using DB_Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SuperMarket.Dtos;

namespace SuperMarket.Controllers
{
	[Route("api/Orders")]
	[ApiController]
	//[Authorize]
	public class OrdersController : ControllerBase
	{

		private readonly IUnitOfWork _context;

		public OrdersController(IUnitOfWork context)
		{
			_context = context;
		}

		[HttpGet]
		//[Authorize(Roles = "Admin")]
		public async Task<IActionResult> GetAll()
		{
			return Ok(await _context.Orders.GetAllAsync(includes: new string[] { "User", "OrderItems" }));
		}
		[HttpGet("{id}")]
		public async Task<IActionResult> Get(string id)
		{
			return Ok(await _context.Orders.GetAsync(x => x.UserId == id, new string[] { "User", "OrderItems" }));
		}
		[HttpPost("Add-Order")]
		public async Task<IActionResult> Post(OrderDto dto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);
			await _context.Orders.Add(new Order { Date = DateTime.Now, State = OrderState.Start, UserId = dto.UserId });
			_context.SaveChanges();
			return Ok();
		}
		[HttpDelete("Delete-Order/{id}")]
		public async Task<IActionResult> Delete(string id)
		{
			var existCart = await _context.Orders.GetAsync(x => x.UserId == id);
			if (existCart == null)
				return BadRequest("This User doesn't have a Order!");

			_context.Orders.Delete(existCart);
			_context.SaveChanges();
			return Ok("Delete Compeleted");
		}

	}
}
