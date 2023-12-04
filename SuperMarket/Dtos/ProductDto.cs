using System.ComponentModel.DataAnnotations;

namespace SuperMarket.Dtos
{
	public class ProductDto
	{
		[Required]
		public string Name { get; set; }
		[Required]
		public string Description { get; set; }
		[Required]
		public IFormFile Image { get; set; }
		[Required]
		public int Quantity { get; set; }
		[Required]
		public double price { get; set; }
		[Required]
		public int CategoryId { get; set; }
	}
}
