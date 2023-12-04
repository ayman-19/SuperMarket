using DB_Core.Models;
using DB_Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SuperMarket.Dtos;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace SuperMarket.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	//[Authorize]
	public class ProductsController : ControllerBase
	{
		private readonly IUnitOfWork _context;
		private readonly IHostingEnvironment _host;

		public ProductsController(IUnitOfWork context, IHostingEnvironment host)
		{
			_context = context;
			_host = host;
		}
		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			return Ok(await _context.Products.GetAllAsync());
		}
		[HttpGet("{id}")]
		public async Task<IActionResult> Get(int id)
		{
			if (!await _context.Products.IsAnyExist(x => x.Id == id))
				return BadRequest("This Product does not exist!");

			return Ok(await _context.Products.GetAsync(x => x.Id == id));
		}
		[HttpPost("Add-Product")]
		//[Authorize(Roles = "Admin")]
		public async Task<IActionResult> AddProduct([FromForm] ProductDto dto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);
			var product = new Product();

			var oldProduct = await _context.Products.GetAsync(x => x.Name.Contains(dto.Name));

			string filename = string.Empty;

			if (dto.Image is not null)
			{
				var upLoad = Path.Combine(_host.WebRootPath, "Images");
				filename = dto.Image.FileName;
				var fullpath = Path.Combine(upLoad, filename);
				await dto.Image.CopyToAsync(new FileStream(fullpath, FileMode.Create));
			}

			product.Name = dto.Name;
			product.Description = dto.Description;
			product.CategoryId = dto.CategoryId;
			product.price = dto.price;
			product.Quantity = dto.Quantity;
			product.Image = filename;

			if(oldProduct != null)
			{
				product.Quantity = product.Quantity + oldProduct.Quantity;
				Edit(oldProduct.Id, product);
				return Ok(product);
			}

			await _context.Products.Add(product);
			_context.SaveChanges();
			return Ok("Add Complete!");
		}
		//[Authorize(Roles = "Admin")]
		[HttpPut("Update-Product/{id}")]
		public async Task<IActionResult> Update(int id, [FromForm] UpdateProductDto dto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var oldProduct = await _context.Products.GetAsync(x => x.Id == id);
			var product = new Product
			{
				Name = dto.Name,
				Description = dto.Description,
				CategoryId = dto.CategoryId,
				price = dto.price,
				Quantity = dto.Quantity,
			};

			string filename = string.Empty;

			if (dto.Image is not null)
			{
				var upLoad = Path.Combine(_host.WebRootPath, "Images");
				filename = dto.Image.FileName;
				var fullpath = Path.Combine(upLoad, filename);
				await dto.Image.CopyToAsync(new FileStream(fullpath, FileMode.Create));
				product.Image = filename;
			}

			if (oldProduct is not null)
			{
				product.Quantity = product.Quantity + oldProduct.Quantity;
				product.Id = oldProduct.Id;
			}

			_context.Products.Update(product);
			_context.SaveChanges();
			return Ok("Update Complete!");
		}
		//[Authorize(Roles = "Admin")]
		[HttpDelete("Delete-Product/{id}")]
		public async Task<IActionResult> Remove(int id)
		{
			if (!await _context.Products.IsAnyExist(x => x.Id == id))
				return BadRequest("This Product does not exist!");

			var product = await _context.Products.GetAsync(x => x.Id == id);

			_context.Products.Delete(product);
			_context.SaveChanges();
			return Ok("Delete Complete!");
		}

		private void Edit(int id, Product product)
		{
			product.Id = id;
			_context.Products.Update(product);
			_context.SaveChanges();
		} 
	}
}
