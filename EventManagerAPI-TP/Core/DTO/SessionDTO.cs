public class SessionDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    public string RoomName { get; set; } = string.Empty;
    public List<SpeakerDTO> Speakers { get; set; } = new();
}
