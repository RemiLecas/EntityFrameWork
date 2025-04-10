public class Session
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    public int EventId { get; set; }
    public Event? Event { get; set; }

    public int RoomId { get; set; }
    public Room? Room { get; set; }

    public ICollection<SessionSpeaker>? SessionSpeakers { get; set; }
    public ICollection<Rating>? Ratings { get; set; }
}
