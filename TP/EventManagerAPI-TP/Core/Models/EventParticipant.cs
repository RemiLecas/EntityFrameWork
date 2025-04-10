public class EventParticipant
{
    public int EventId { get; set; }
    public Event? Event { get; set; }

    public int ParticipantId { get; set; }
    public Participant? Participant { get; set; }

    public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
    public AttendanceStatus AttendanceStatus { get; set; } = AttendanceStatus.Pending;
}

public enum AttendanceStatus
{
    Pending,
    Confirmed,
    Attended,
    Absent,
    Cancelled
}
