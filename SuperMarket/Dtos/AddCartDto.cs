using System.ComponentModel.DataAnnotations;

namespace SuperMarket.Dtos
{
	public class AddCartDto
	{
		[Required]
		public string UserId { get; set; }
	}
}
