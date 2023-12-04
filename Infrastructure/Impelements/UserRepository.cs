using DB_Core.Models;
using DB_Core.Services;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Impelements
{
	public class UserRepository : Repository<User>, IUserRepository
	{
		public UserRepository(SuperMarketContext context) : base(context)
		{
		}
	}
}
