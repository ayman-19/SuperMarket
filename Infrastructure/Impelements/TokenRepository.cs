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
	internal class TokenRepository : Repository<Token>, ITokenRepository
	{
		public TokenRepository(SuperMarketContext context) : base(context)
		{
		}
	}
}
