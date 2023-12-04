using DB_Core.Models;

namespace SuperMarket.Dtos
{
	public class CategoryDetails
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public string Image { get; set; }
		public virtual List<Product>? Products { get; set; } = new List<Product>();
	}
}
