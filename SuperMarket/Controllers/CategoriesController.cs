using DB_Core.Models;
using DB_Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SuperMarket.Dtos;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace SuperMarket.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [AllowAnonymous]
    public class CategoriesController : ControllerBase
    {
        private readonly IUnitOfWork _context;
        private readonly IHostingEnvironment _host;

        public CategoriesController(IUnitOfWork context, IHostingEnvironment host)
        {
            _context = context;
            _host = host;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _context.Categories.GetAllAsync());
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            if (!await _context.Categories.IsAnyExist(x => x.Id == id))
                return BadRequest("This Category does not exist, Write another name!");
            var category = await _context.Categories.GetAsync(x => x.Id == id, new string[] { "Products" });

			return Ok(new CategoryDetails
            {
                Products = category.Products!.ToList(),
                Name = category.Name,
                Description = category.Description,
                Image = category.Image,
            });
        }
        [HttpPost("Add-Category")]
		//[Authorize(Roles = "Admin")]
		public async Task<IActionResult> AddCategory([FromForm] GategoryDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string filename = string.Empty;

            if (dto.Image is not null)
            {
                var upLoad = Path.Combine(_host.WebRootPath, "Images");
                filename = dto.Image.FileName;
                var fullpath = Path.Combine(upLoad, filename);
                await dto.Image.CopyToAsync(new FileStream(fullpath, FileMode.Create));
            }

            await _context.Categories.Add(new Category
            {
                Name = dto.Name,
                Description = dto.Description,
                Image = filename,
            });
            _context.SaveChanges();
            return Ok("Add Complete!");
        }
		//[Authorize(Roles = "Admin")]
		[HttpPut("Update-Category/{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] UpdateCategoryDto dto) {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var cat = await _context.Categories.GetAsync(x => x.Id == id);
            if(cat is null)
                return NotFound();
			cat.Name = dto.Name;
			cat.Description = dto.Description;

            string filename = string.Empty;

            if (dto.Image is not null)
            {
                var upLoad = Path.Combine(_host.WebRootPath, "Images");
                filename = dto.Image.FileName;
                var fullpath = Path.Combine(upLoad, filename);
                await dto.Image.CopyToAsync(new FileStream(fullpath, FileMode.Create));
                cat.Image = filename;
            }

            _context.Categories.Update(cat);
            _context.SaveChanges();
            return Ok("Update Complete!");
        }
		//[Authorize(Roles = "Admin")]
		[HttpDelete("Delete-Category/{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            if (!await _context.Categories.IsAnyExist(x => x.Id == id))
                return BadRequest("This Category does not exist!");

            var category = await _context.Categories.GetAsync(x => x.Id == id);

            _context.Categories.Delete(category);
            _context.SaveChanges();
            return Ok("Delete Complete!");
        }

    }
   
}
