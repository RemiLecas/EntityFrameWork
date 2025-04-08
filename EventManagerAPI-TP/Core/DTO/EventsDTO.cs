public class EventCreateDTO
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int Status { get; set; }
    public int Category { get; set; }
    public int LocationId { get; set; }
}

public class EventReadDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public string LocationName { get; set; } = string.Empty;
    public List<SessionDTO> Sessions { get; set; } = new();
    public List<ParticipantDTO> Participants { get; set; } = new();
}

public class EventUpdateDTO
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int Status { get; set; }
    public int Category { get; set; }
    public int LocationId { get; set; }
}
