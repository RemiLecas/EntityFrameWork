public class RatingReadDTO
{
    public int Id { get; set; }
    public int Score { get; set; }
    public string Comment { get; set; } = string.Empty;
    public int SessionId { get; set; }
    public SessionReadDTO? Session { get; set; }
}
