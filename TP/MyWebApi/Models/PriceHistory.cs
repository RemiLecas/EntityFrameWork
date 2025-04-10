public class PriceHistory
{
    public int PriceHistoryId { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
    public decimal Price { get; set; }
    public DateTime ChangedAt { get; set; }
}
