public class ParticipantReadDTO
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string? Company { get; set; }
    public string? JobTitle { get; set; }
}

public class ParticipantCreateDTO
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string? Company { get; set; }
    public string? JobTitle { get; set; }
}

public class ParticipantUpdateDTO
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string? Company { get; set; }
    public string? JobTitle { get; set; }
}

