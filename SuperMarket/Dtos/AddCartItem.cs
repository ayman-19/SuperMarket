using System.ComponentModel.DataAnnotations;

namespace SuperMarket.Dtos
{
	public class AddCartItem
	{
		[Required]
		public int CartId { get; set; }
		[Required]
		public int ProductId { get; set; }
		[Required]
		public int Count { get; set; }
	}
}
