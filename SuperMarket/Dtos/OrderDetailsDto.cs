namespace SuperMarket.Dtos
{
	public class OrderDetailsDto
	{
        public DateTime Date { get; set; }
        public List<ProductDetailsDto> Products { get; set; }
        public double Total { get; set; }
    }
}
