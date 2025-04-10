namespace EventManagerAPI_TP.Core.DTO {
    public class EventCreateDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Status { get; set; }
        public int CategoryId { get; set; }
        public int LocationId { get; set; }
        public List<int>? SessionIds { get; set; } = new List<int>();
        public int RoomId { get; set; }
        public List<int>? ParticipantIds { get; set; } = new List<int>();
        public List<int>? SpeakerIds { get; set; } = new List<int>();
    }

    public class EventReadDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Status { get; set; }
        public CategoryReadDTO Category { get; set; }
        public LocationReadDTO Location { get; set; } = new LocationReadDTO();
        public List<SessionReadDTO> Sessions { get; set; } = new List<SessionReadDTO>();
        public List<ParticipantReadDTO> Participants { get; set; } = new List<ParticipantReadDTO>();
    }

    public class EventUpdateDTO
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Status { get; set; }
        public int CategoryId { get; set; }
        public int LocationId { get; set; }
        public List<int>? SessionIds { get; set; } = new List<int>();
    }
}

