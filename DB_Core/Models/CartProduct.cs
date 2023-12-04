using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DB_Core.Models
{
    [PrimaryKey(nameof(CartId),nameof(ProductId))]
    public class CartProduct
    {
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public bool Out { get; set; }
        public int Count { get; set; }
        public double TotalAmount { get; set; }
        [ForeignKey(nameof(CartId))]
        [JsonIgnore]
        public virtual Cart? Cart { get; set; }
        [ForeignKey(nameof(ProductId))]
		[JsonIgnore]
		public virtual Product? Product { get; set; }
    }
}
