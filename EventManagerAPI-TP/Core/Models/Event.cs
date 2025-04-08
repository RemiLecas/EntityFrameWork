public class Event
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public EventStatus Status { get; set; } = EventStatus.Planned;
    public EventCategory Category { get; set; } = EventCategory.Other;

    public int LocationId { get; set; }
    public Location? Location { get; set; }

    public ICollection<EventParticipant>? EventParticipants { get; set; }
    public ICollection<Session>? Sessions { get; set; }
}

public enum EventStatus
{
    Planned,
    Canceled,
    Completed
}

public enum EventCategory
{
    Conference,
    Workshop,
    Meetup,
    Webinar,
    Training,
    Other
}