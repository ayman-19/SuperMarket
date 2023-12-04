using System.ComponentModel.DataAnnotations;

namespace DB_Core.Models
{
	public class Register
	{
		[Required]
		public string FirstName { get; set; }
		[Required]
		public string LastName { get; set; }
		[Required]
		public string Address { get; set; }
		[Required]
		public string UserName { get; set; }
		[Required,DataType(DataType.EmailAddress)]
        public string Email { get; set; }
		[Required,DataType(DataType.Password)]
		public string Password { get; set; }
		[Required, DataType(DataType.Password),Compare("Password")]
		public string ConfirmPassword { get; set; }
    }
}
