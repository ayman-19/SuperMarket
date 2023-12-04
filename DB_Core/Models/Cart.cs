using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DB_Core.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public double Total { get; set; }
        [JsonIgnore]
        public virtual List<CartProduct>? CartProducts { get; set; } = new List<CartProduct>();
        [ForeignKey(nameof(UserId))]
		[JsonIgnore]
		public User? User { get; set; }
    }
}
