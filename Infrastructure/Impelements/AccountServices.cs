using DB_Core.Models;
using DB_Core.Services;
using Infrastructure.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Impelements
{
	public class AccountServices : IAccountServices
	{
		private readonly UserManager<User> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IUnitOfWork unitOfWork;
		private readonly JWT _jwt;

		public AccountServices(UserManager<User> userManager,RoleManager<IdentityRole> roleManager, IOptions<JWT> jwt,IUnitOfWork unitOfWork)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			this.unitOfWork = unitOfWork;
			_jwt = jwt.Value;
		}
		public async Task<UserToken> RegisterAsync(Register model)
		{
			if (await _userManager.FindByEmailAsync(model.Email) is not null)
				return new UserToken { Massage = "Email Already Register!" };

			if (await _userManager.FindByNameAsync(model.UserName) is not null)
				return new UserToken { Massage = "UserName Already Exist!" };

			var user = new User
			{
				UserName = model.UserName,
				Email = model.Email,
				FirstName = model.FirstName,
				LastName = model.LastName,
				Address = model.Address,
			};

			var result = await _userManager.CreateAsync(user, model.Password);
			if (!result.Succeeded)
			{
				var errors = string.Empty;
				foreach (var error in result.Errors)
					errors += error.Description + ", ";
				return new UserToken { Massage = errors };
            }

			await _userManager.AddToRoleAsync(user, "User");
			RefreshToken? refToken = CreateRefreshTokenAsync();
			var token = await CreateTokenAsync(user);

			var usertoken = new UserToken()
			{
				RefreshTokenExpirations = refToken.ExpiresOn,
				Roles = new List<string> { "User" },
				Token = new JwtSecurityTokenHandler().WriteToken(token),
				TokenExpirations = token.ValidTo,
				UserName = user.UserName,
				IsAuthenticated = true,
				RefreshToken = refToken.Token
			};
			await unitOfWork.Tokens.Add(new Token { UserId = user.Id, TokenValue = usertoken.Token });
			unitOfWork.SaveChanges();
			return usertoken;
		}
		public async Task<UserToken> LoginAsync(Login model)
		{
			var user = await _userManager.FindByEmailAsync(model.Email);
			if (user is  null||
				!await _userManager.CheckPasswordAsync(user,model.Password))
				return new UserToken { Massage = "Email Or Password InValed!" };

			var token = await CreateTokenAsync(user);
			var userToken = new UserToken()
			{
				Token = new JwtSecurityTokenHandler().WriteToken(token),
				Roles = (await _userManager.GetRolesAsync(user)).ToList(),
				UserName = user.UserName,
				IsAuthenticated = true,
				TokenExpirations = token.ValidTo
			};
			if(user.RefreshTokens.Any(x=>x.IsActive))
			{
				var isActive = user.RefreshTokens.FirstOrDefault(x => x.IsActive);
				userToken.RefreshTokenExpirations = isActive.ExpiresOn;
				userToken.RefreshToken = isActive.Token;
			}
			else
			{
				var refToken =  CreateRefreshTokenAsync();
				user.RefreshTokens.Add(refToken);

				await _userManager.UpdateAsync(user);
				userToken.RefreshTokenExpirations = refToken.ExpiresOn;
				userToken.RefreshToken = refToken.Token;
			}
			if (await unitOfWork.Tokens.IsAnyExist(x => x.UserId == user.Id))
				unitOfWork.Tokens.Update(new Token { UserId = user.Id, TokenValue = userToken.Token });
			else
				await unitOfWork.Tokens.Add(new Token { UserId = user.Id, TokenValue = userToken.Token });
			unitOfWork.SaveChanges();
			

			return userToken;
		}
		public async Task<string> AddRoleAsync(AddRole role)
		{
			var user = await _userManager.FindByIdAsync(role.UserID);
			if (user is null || !await _roleManager.RoleExistsAsync(role.RoleName))
				return "User Or Role InValid!";
			if (await _userManager.IsInRoleAsync(user, role.RoleName))
				return "User already assigned this Role";
			var result = await _userManager.AddToRoleAsync(user, role.RoleName);
			return result.Succeeded ? string.Empty : "Error";
		}

		public async Task<UserToken> RefreshTokenAsync(string token)
		{
			var user = await _userManager.Users.FirstOrDefaultAsync(x => x.RefreshTokens.Any(y => y.Token == token));

			if (user is null)
				return new UserToken { Massage = "User Or Token Invalid!" };

			var tokenisrefresh = user.RefreshTokens.FirstOrDefault(x => x.Token == token);

			if(!tokenisrefresh.IsActive)
				return new UserToken { Massage = "Token Not Active!" };

			tokenisrefresh.RevokeOn = DateTime.UtcNow;

			var newtokenRefresh = CreateRefreshTokenAsync();
			user.RefreshTokens.Add(newtokenRefresh);
			await _userManager.UpdateAsync(user);
			var newtoken = await CreateTokenAsync(user);
			return new UserToken
			{
				IsAuthenticated = true,
				RefreshToken = newtokenRefresh.Token,
				Token = new JwtSecurityTokenHandler().WriteToken(newtoken),
				Roles = (await _userManager.GetRolesAsync(user)).ToList(),
				RefreshTokenExpirations = newtokenRefresh.ExpiresOn,
				TokenExpirations = newtoken.ValidTo,
				UserName = user.UserName
			};
		}
		public async Task<bool> RevokeTokenAsync(string token)
		{

			var user = await _userManager.Users.FirstOrDefaultAsync(x => x.RefreshTokens.Any(y => y.Token == token));

			if (user is null)
				return false;

			var tokenrevoke = user.RefreshTokens.FirstOrDefault(x => x.Token == token);

			if (!tokenrevoke.IsActive)
				return false;

			tokenrevoke.RevokeOn = DateTime.UtcNow;
			await _userManager.UpdateAsync(user);

			return true;
		}
		private async Task<JwtSecurityToken> CreateTokenAsync(User user)
		{
			var userClaims = await _userManager.GetClaimsAsync(user);
			var userRole = await _userManager.GetRolesAsync(user);
			var roleClaims = new List<Claim>();
			foreach (var role in userRole)
				roleClaims.Add(new Claim(ClaimTypes.Role, role));
			var claims = new List<Claim>()
			{
				new Claim(ClaimTypes.Email,user.Email),
				new Claim(ClaimTypes.Name,user.UserName),
				new Claim(ClaimTypes.PrimarySid,user.Id),
			};
			claims.AddRange(roleClaims);
			claims.AddRange(userClaims);
			var symmitreckey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
			var signingCredentials = new SigningCredentials(symmitreckey, SecurityAlgorithms.HmacSha256);
			var token = new JwtSecurityToken(issuer: _jwt.Issuer, audience: _jwt.Audience, claims: claims, expires: DateTime.Now.AddDays(_jwt.DurationInDays), signingCredentials: signingCredentials);
			return token;
		}
		private RefreshToken CreateRefreshTokenAsync()
		{
			var token = new byte[32];
			var generator = new RNGCryptoServiceProvider();
			generator.GetBytes(token);

			var refreshToken = new RefreshToken()
			{
				ExpiresOn = DateTime.UtcNow.AddDays(_jwt.DurationInDays),
				Token = Convert.ToBase64String(token),
				CreateOn = DateTime.UtcNow
			};
			return refreshToken;
		}
	}
}
