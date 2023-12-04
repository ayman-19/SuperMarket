using DB_Core.Models;
using DB_Core.Services;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Impelements
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SuperMarketContext _context;

        public UnitOfWork(SuperMarketContext context)
        {
            _context = context;
            //_context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            Carts = new CartRepository(_context);
            Categories = new CategoryRepository(_context);
            Products = new ProductRepository(_context);
			CartItems = new CartItem(_context);
			OrderItems = new OrderIItem(_context);
			Orders = new OrderRepository(_context);
			Users = new UserRepository(_context);
			Tokens = new TokenRepository(_context);
		}

        public ICartRepository Carts { get; }
        public IOrderRepository Orders { get; }

        public ICategoryRepository Categories { get; }

        public IProductRepository Products { get; }

		public ICartItem CartItems { get; }

		public IOrderItem OrderItems { get; }

		public IUserRepository Users { get; }

		public ITokenRepository Tokens { get; }

		public void Dispose()
        {
            _context.Dispose();
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }
    }
}
