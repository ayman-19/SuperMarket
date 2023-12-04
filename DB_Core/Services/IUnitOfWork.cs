using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Core.Services
{
    public interface IUnitOfWork : IDisposable
    {
        int SaveChanges();
        public ICartRepository Carts { get;}
        public IUserRepository Users { get;}
        public ICategoryRepository Categories { get;}
        public IProductRepository Products { get;}
        public ICartItem CartItems { get;}
        public IOrderItem OrderItems { get;}
		public IOrderRepository Orders { get; }
		public ITokenRepository Tokens { get; }
	}
}
