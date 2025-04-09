public class SessionSpeaker
{
    public int SessionId { get; set; }
    public Session? Session { get; set; }

    public int SpeakerId { get; set; }
    public Speaker? Speaker { get; set; }

    public string Role { get; set; } = string.Empty;
}

public enum SpeakerRole
{
    Speaker,
    Moderator,
    Panelist,
    Keynote,
    Guest
}
