using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SuperMarket.Dtos
{
	public class OrderDto
	{
		[Required]
        public string UserId { get; set; }
    }
}
