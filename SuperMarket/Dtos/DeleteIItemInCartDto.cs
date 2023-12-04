using System.ComponentModel.DataAnnotations;

namespace SuperMarket.Dtos
{
	public class DeleteIItemInCartDto
	{
		[Required]
		public int CartId { get; set; }
		[Required]
		public int ProductId { get; set; }
	}
}
