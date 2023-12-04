using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Core.Models
{
	public class AddRole
	{
		[Required]
		public string UserID { get; set; }
		[Required]
		public string RoleName { get; set; }
	}
}
