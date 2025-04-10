public class SessionCreateDTO
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    
    // Liens vers les entités associées
    public int EventId { get; set; }
    public int RoomId { get; set; }
}

public class SessionReadDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string EventTitle { get; set; } = string.Empty;
    public string RoomName { get; set; } = string.Empty;
    public List<SpeakerReadDTO> Speakers { get; set; } = new List<SpeakerReadDTO>();
    public List<RatingReadDTO> Ratings { get; set; } = new List<RatingReadDTO>();
}


public class SessionUpdateDTO
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    
    public int EventId { get; set; }
    public int RoomId { get; set; }
}
