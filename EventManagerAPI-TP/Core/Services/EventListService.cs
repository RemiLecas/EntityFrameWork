using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public class EventListService : IEventListService
{
    private readonly ApplicationDbContext _context;

    public EventListService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<EventListResult> GetEventsAsync(DateTime? startDate, DateTime? endDate, int? locationId, int? category, int? status, int page, int pageSize)
    {
        var query = _context.Events
            .Include(e => e.Location)
            .Include(e => e.Category)
            .Include(e => e.Sessions)
                .ThenInclude(s => s.Room)
            .Include(e => e.Sessions)
                .ThenInclude(s => s.SessionSpeakers)
                    .ThenInclude(ss => ss.Speaker)
            .Include(e => e.EventParticipants)
                .ThenInclude(ep => ep.Participant)
            .AsQueryable();

        if (startDate.HasValue)
            query = query.Where(e => e.StartDate >= startDate.Value);
        if (endDate.HasValue)
            query = query.Where(e => e.EndDate <= endDate.Value);
        if (locationId.HasValue)
            query = query.Where(e => e.LocationId == locationId.Value);
        if (category.HasValue)
            query = query.Where(e => e.CategoryId == category.Value);
        if (status.HasValue)
            query = query.Where(e => e.Status == (EventStatus)status.Value);

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);  // Calculer le total des pages

    var events = await query.Skip((page - 1) * pageSize).Take(pageSize)
        .Select(e => new EventReadDTO
        {
            Id = e.Id,
            Title = e.Title,
            Description = e.Description,
            StartDate = e.StartDate,
            EndDate = e.EndDate,
            Location = e.Location != null ? new LocationReadDTO
            {
                Id = e.Location.Id,
                Name = e.Location.Name,
                Address = e.Location.Address,
                City = e.Location.City,
                Country = e.Location.Country,
                Capacity = e.Location.Capacity
            } : null,
            Category = e.Category != null ? new CategoryReadDTO
            {
                Id = e.Category.Id,
                Name = e.Category.Name
            } : null,
            Sessions = e.Sessions.Select(s => new SessionReadDTO
            {
                Id = s.Id,
                Title = s.Title,
                Description = s.Description,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                RoomName = s.Room != null ? s.Room.Name : "",
                Speakers = s.SessionSpeakers.Select(ss => new SpeakerReadDTO
                {
                    Id = ss.Speaker.Id,
                    FirstName = ss.Speaker.FirstName,
                    LastName = ss.Speaker.LastName,
                    Company = ss.Speaker.Company
                }).ToList()
            }).ToList(),
            Participants = e.EventParticipants.Select(ep => new ParticipantReadDTO
            {
                Id = ep.Participant.Id,
                FirstName = ep.Participant.FirstName,
                LastName = ep.Participant.LastName,
                Email = ep.Participant.Email
            }).ToList()
        }).ToListAsync();

        return new EventListResult
        {
            Events = events,
            TotalPages = totalPages
        };
    }
}
