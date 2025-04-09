using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public class EventService : IEventService
{
    private readonly ApplicationDbContext _context;

    public EventService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Event> CreateEventAsync(EventCreateDTO dto)
    {
        var location = await _context.Locations.FindAsync(dto.LocationId);
        if (location == null)
        {
            throw new ArgumentException("Location not found");
        }

        var room = await _context.Rooms.FindAsync(dto.RoomId);
        if (room == null)
        {
            throw new ArgumentException("Room not found");
        }

        var category = await _context.Categories.FindAsync(dto.CategoryId);
        if (category == null)
        {
            throw new ArgumentException("Category not found");
        }

        var @event = new Event
        {
            Title = dto.Title,
            Description = dto.Description,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            Status = (EventStatus)dto.Status,
            CategoryId = dto.CategoryId,
            LocationId = dto.LocationId,
            RoomId = dto.RoomId,
        };

        _context.Events.Add(@event);
        await _context.SaveChangesAsync();

        return @event;
    }

    public async Task<IEnumerable<EventReadDTO>> GetEventsAsync(DateTime? startDate, DateTime? endDate, int? locationId, int? category, int? status, int page, int pageSize)
    {
        var query = _context.Events
            .Include(e => e.Location)
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
            query = query.Where(e => e.CategoryId  == category.Value);
        if (status.HasValue)
            query = query.Where(e => e.Status == (EventStatus)status.Value);

        var totalCount = await query.CountAsync();
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

        return events;
    }

    public async Task<EventReadDTO> GetEventByIdAsync(int id)
    {
        var @event = await _context.Events
            .Include(e => e.Location)
            .Include(e => e.Sessions)
                .ThenInclude(s => s.Room)
            .Include(e => e.Sessions)
                .ThenInclude(s => s.SessionSpeakers)
                    .ThenInclude(ss => ss.Speaker)
            .Include(e => e.EventParticipants)
                .ThenInclude(ep => ep.Participant)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (@event == null)
            return null;

        return new EventReadDTO
        {
            Id = @event.Id,
            Title = @event.Title,
            Description = @event.Description,
            StartDate = @event.StartDate,
            EndDate = @event.EndDate,
            Location = @event.Location != null ? new LocationReadDTO
            {
                Id = @event.Location.Id,
                Name = @event.Location.Name,
                Address = @event.Location.Address,
                City = @event.Location.City,
                Country = @event.Location.Country,
                Capacity = @event.Location.Capacity
            } : null,
            Sessions = @event.Sessions?.Select(s => new SessionReadDTO
            {
                Id = s.Id,
                Title = s.Title,
                Description = s.Description,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                RoomName = s.Room?.Name ?? "Unknown",
                Speakers = s.SessionSpeakers?.Select(ss => new SpeakerReadDTO
                {
                    Id = ss.Speaker.Id,
                    FirstName = ss.Speaker.FirstName,
                    LastName = ss.Speaker.LastName,
                    Company = ss.Speaker.Company
                }).ToList() ?? new List<SpeakerReadDTO>()
            }).ToList() ?? new List<SessionReadDTO>(),
            Participants = @event.EventParticipants?.Select(ep => new ParticipantReadDTO
            {
                Id = ep.Participant.Id,
                FirstName = ep.Participant.FirstName,
                LastName = ep.Participant.LastName,
                Email = ep.Participant.Email
            }).ToList() ?? new List<ParticipantReadDTO>()
        };
    }

    public async Task<bool> UpdateEventAsync(int id, EventUpdateDTO eventUpdateDTO)
    {
        var existingEvent = await _context.Events.FindAsync(id);
        if (existingEvent == null)
        {
            return false;
        }

        existingEvent.Title = eventUpdateDTO.Title;
        existingEvent.Description = eventUpdateDTO.Description;
        existingEvent.StartDate = eventUpdateDTO.StartDate;
        existingEvent.EndDate = eventUpdateDTO.EndDate;
        existingEvent.Status = (EventStatus)eventUpdateDTO.Status;
        existingEvent.CategoryId = eventUpdateDTO.CategoryId;
        existingEvent.LocationId = eventUpdateDTO.LocationId;

        _context.Entry(existingEvent).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteEventAsync(int id)
    {
        var @event = await _context.Events.FindAsync(id);
        if (@event == null)
        {
            return false;
        }

        _context.Events.Remove(@event);
        await _context.SaveChangesAsync();

        return true;
    }
}
