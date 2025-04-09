public class RoomCreateDTO
{
    public string Name { get; set; }
    public int Capacity { get; set; }
    public int LocationId { get; set; }
}

public class RoomUpdateDTO
{
    public string Name { get; set; }
    public int Capacity { get; set; }
}

public class RoomReadDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Capacity { get; set; }
    public LocationReadDTO Location { get; set; }
    public List<SessionReadDTO> Sessions { get; set; }
}