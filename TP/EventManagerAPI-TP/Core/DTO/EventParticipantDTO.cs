public class EventParticipantsReadDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public AttendanceStatus AttendanceStatus { get; set; }
}

public class EventParticipantsRegistrationDTO
{
    public int EventId { get; set; }
    public int ParticipantId { get; set; }
}
