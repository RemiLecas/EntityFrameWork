public class Product
{
    public int ProductId { get; set; }
    public string Name { get; set; }
    public int CategoryId { get; set; }
    public Price Price { get; set; }
    public Category Category { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; }
    public ICollection<ProductRating> ProductRatings { get; set; } = new List<ProductRating>();

    public ICollection<Stock> Stocks { get; set; } = new List<Stock>();

    public ICollection<PriceHistory> PriceHistories { get; set; } = new List<PriceHistory>();

}
