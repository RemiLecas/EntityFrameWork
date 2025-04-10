public class Speaker
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Bio { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? Company { get; set; }

    public ICollection<SessionSpeaker>? SessionSpeakers { get; set; }
}
