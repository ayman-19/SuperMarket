using System.ComponentModel.DataAnnotations;

namespace SuperMarket.Dtos
{
	public class UserCheckOut
	{
		[Required]
        public string UserId { get; set; }
    }
}
