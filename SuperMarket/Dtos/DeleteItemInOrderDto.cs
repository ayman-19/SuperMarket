using System.ComponentModel.DataAnnotations;

namespace SuperMarket.Dtos
{
	public class DeleteItemInOrderDto
	{
		[Required]
		public int OrderId { get; set; }
		[Required]
		public int ProductId { get; set; }
	}
}
