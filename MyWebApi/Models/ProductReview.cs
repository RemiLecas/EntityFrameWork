public class ProductRating
{
    public int ProductRatingId { get; set; }
    public int Rating { get; set; }
    public int ProductId { get; set; }
    public int CustomerId { get; set; }
    public Customer Customer { get; set; }
    public string Comment { get; set; }
    public Product Product { get; set; }
}
