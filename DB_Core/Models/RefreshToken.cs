using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Core.Models
{
    [Owned]
	public class RefreshToken
	{
        public string Token { get; set; }
        public DateTime ExpiresOn { get; set; }
        public bool IsExpires => ExpiresOn <= DateTime.UtcNow;
        public bool IsActive => RevokeOn == null && !IsExpires ;
        public DateTime CreateOn { get; set; }
        public DateTime? RevokeOn { get; set; }

    }
}
