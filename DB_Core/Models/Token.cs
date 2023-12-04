using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Core.Models
{
	public class Token
	{
		[Key]
        public string UserId { get; set; }
        public string TokenValue { get; set; }
    }
}
