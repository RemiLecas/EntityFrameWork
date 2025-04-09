public class Event
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public EventStatus Status { get; set; } = EventStatus.Planned;
    public int CategoryId { get; set; }
    public Category Category { get; set; }
    public int LocationId { get; set; }
    public Location? Location { get; set; }
    public int? RoomId { get; set; }
    public Room? Room { get; set; } 

    public ICollection<EventParticipant> EventParticipants { get; set; } = new List<EventParticipant>();
    public ICollection<Session> Sessions { get; set; } = new List<Session>();
}

public enum EventStatus
{
    Planned,
    Canceled,
    Completed
}

public class EventListResult
{
    public IEnumerable<EventReadDTO> Events { get; set; }
    public int TotalPages { get; set; }
}
