public class Rating
{
    public int Id { get; set; }

    public int SessionId { get; set; }
    public Session? Session { get; set; }

    public int ParticipantId { get; set; }
    public Participant? Participant { get; set; }

    public int Score { get; set; }
    public string? Comment { get; set; }
}
