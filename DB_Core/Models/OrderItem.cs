using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace DB_Core.Models
{
    [PrimaryKey(nameof(OrderId), nameof(ProductId))]
    public class OrderItem
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public bool Out { get; set; }
        public int Count { get; set; }
        public double TotalAmount { get; set; }
        [ForeignKey(nameof(OrderId))]
        public Order? Order { get; set; }
        [ForeignKey(nameof(ProductId))]
        public Product? Product { get; set; }
    }
}
