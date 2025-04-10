public class Room
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Capacity { get; set; }

    public int LocationId { get; set; }
    public Location? Location { get; set; }
    public ICollection<Session>? Sessions { get; set; }
}
