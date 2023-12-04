using System.ComponentModel.DataAnnotations;

namespace SuperMarket.Dtos
{
	public class ItemInOrderDto
	{
		[Required]
		public int OrderId { get; set; }
		[Required]
		public int ProductId { get; set; }
		[Required]
		public int Count { get; set; }
	}
}
