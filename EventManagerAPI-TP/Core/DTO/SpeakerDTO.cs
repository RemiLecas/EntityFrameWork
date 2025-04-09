public class SpeakerCreateDTO
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Bio { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? Company { get; set; }
}

public class SpeakerReadDTO
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Bio { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? Company { get; set; }
    public string Role { get; set; } = string.Empty;
}
