using DB_Core.Models;
using DB_Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SuperMarket.Dtos;

namespace SuperMarket.Controllers
{
	[Route("api/Accounts")]
	[ApiController]
	public class AccountsController : ControllerBase
	{
		private readonly IAccountServices _accountServices;
		private readonly UserManager<User> _userManager;
		private readonly IUnitOfWork _context;

		public AccountsController(IAccountServices accountServices, UserManager<User> userManager,IUnitOfWork context)
		{
			_accountServices = accountServices;
			_userManager = userManager;
			_context = context;
		}

		[HttpPost("Register")]
		public async Task<IActionResult> Register(Register model)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var result = await _accountServices.RegisterAsync(model);

			if (!result.IsAuthenticated)
				return BadRequest(result.Massage);

			SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpirations);
			return Ok(result);
		}
		[HttpPost("Login")]
		public async Task<IActionResult> Login(Login model)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);
			UserToken? result = await _accountServices.LoginAsync(model);
			if (!result.IsAuthenticated)
				return BadRequest(result.Massage);
			SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpirations);
			var user = await _userManager.FindByEmailAsync(model.Email);
			if (user is not null && !await _context.Carts.IsAnyExist(x => x.UserId == user.Id))
				{
				using (var httpclient =  new HttpClient())
				{
					var url = "https://localhost:7255/api/Carts/Add-Cart";
					var response = await httpclient.PostAsJsonAsync(url,new Cart { UserId = user.Id });
					_context.SaveChanges();
				}
			}
			return Ok(result);
		}
		[HttpPost("Add-Role")]
		[Authorize(Roles ="Admin")]
		public async Task<IActionResult> AddRole(AddRole role)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var result = await _accountServices.AddRoleAsync(role);

			if (!string.IsNullOrEmpty(result))
				return BadRequest(result);
			return Ok(result);
		}
		[HttpGet("Refresh-Token")]
		[Authorize]
		public async Task<IActionResult> RefreshToken()
		{
			var token = Request.Cookies["RefreshToken"];
			if (!string.IsNullOrEmpty(token))
				return BadRequest(token);
			var result = await _accountServices.RefreshTokenAsync(token!);
			if (!result.IsAuthenticated)
				return BadRequest(result.Massage);
			SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpirations);
			return Ok(result);
		}
		[HttpGet("Revoke-Token")]
		[Authorize]
		public async Task<IActionResult> RevokeToken()
		{
			var token = Request.Cookies["RefreshToken"];
			var result = await _accountServices.RevokeTokenAsync(token!);
			if (!result)
				return BadRequest("Token InValid");
			return Ok(result);
		}
		private void SetRefreshTokenInCookie(string refreshToken, DateTime Expire)
		{
			var cookie = new CookieOptions
			{
				HttpOnly = true,
				Expires = Expire.ToLocalTime(),
			};
			Response.Cookies.Append("RefreshToken", refreshToken, cookie);
		}
	}
}
