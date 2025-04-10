public class Participant
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Company { get; set; }
    public string? JobTitle { get; set; }

    public ICollection<EventParticipant>? EventParticipants { get; set; }
    public ICollection<Rating>? Ratings { get; set; }
}
