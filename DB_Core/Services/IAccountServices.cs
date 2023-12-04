using DB_Core.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Core.Services
{
	public interface IAccountServices
	{
		Task<UserToken> RegisterAsync(Register model);
		Task<UserToken> LoginAsync(Login model);
		Task<UserToken> RefreshTokenAsync(string token);
		Task<bool> RevokeTokenAsync(string token);
		Task<string> AddRoleAsync(AddRole role);

	}
}
