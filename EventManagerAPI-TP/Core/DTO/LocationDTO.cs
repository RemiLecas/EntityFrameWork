public class LocationReadDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public int Capacity { get; set; }
    public List<RoomReadDTO> Rooms { get; set; } = new List<RoomReadDTO>();
    public List<EventReadDTO> Events { get; set; } = new List<EventReadDTO>();
}

public class LocationCreateDTO
{
    public string Name { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public int Capacity { get; set; }
    public string ZipCode { get; set; }
}

public class LocationUpdateDTO
{
    public string Name { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public int Capacity { get; set; }
    public string ZipCode { get; set; }
}

public class LocationDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public int Capacity { get; set; }
    public string ZipCode { get; set; }
    
    // Vous pouvez inclure les rooms et événements ici si nécessaire
    public ICollection<RoomReadDTO> Rooms { get; set; } = new List<RoomReadDTO>();
    public ICollection<EventReadDTO> Events { get; set; } = new List<EventReadDTO>();
}


