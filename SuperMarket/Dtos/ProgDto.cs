using System.ComponentModel.DataAnnotations;

namespace SuperMarket.Dtos
{
	public class ProgDto
	{
        [Required]
        public int OrderId { get; set; }
		[Required]
		public string UserId { get; set; }
    }
}
