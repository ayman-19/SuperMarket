using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DB_Core.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public double TotalPrice { get; set; }
        public OrderState State { get; set; }
        public string UserId { get; set; }
        [JsonIgnore]
        public List<OrderItem> OrderItems { get; set; } = new();
        [ForeignKey(nameof(UserId))]
		[JsonIgnore]
		public User? User { get; set; }
    }
}
