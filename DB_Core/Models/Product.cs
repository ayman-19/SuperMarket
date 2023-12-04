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
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int Quantity { get; set; }
        public double price { get; set; }
        public int CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        [JsonIgnore]
        public virtual Category Category { get; set; }
		[JsonIgnore]
		public virtual List<CartProduct>? CartProducts { get; set; } = new List<CartProduct>();
		[JsonIgnore]
		public virtual List<OrderItem>? OrderItems { get; set; } = new List<OrderItem>();

    }
}
