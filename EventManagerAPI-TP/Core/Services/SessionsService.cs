using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
public class SessionService : ISessionService
{
    private readonly ApplicationDbContext _context;

    public SessionService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SessionReadDTO> CreateSessionAsync(SessionCreateDTO dto)
    {
        var session = new Session
        {
            Title = dto.Title,
            Description = dto.Description,
            StartTime = dto.StartTime,
            EndTime = dto.EndTime,
            EventId = dto.EventId,
            RoomId = dto.RoomId
        };

        var eventEntity = await _context.Events.FindAsync(dto.EventId);
        var roomEntity = await _context.Rooms.FindAsync(dto.RoomId);

        if (eventEntity == null || roomEntity == null)
        {
            throw new InvalidOperationException("Event or Room not found.");
        }

        session.Event = eventEntity;
        session.Room = roomEntity;

        _context.Sessions.Add(session);
        await _context.SaveChangesAsync();

        return new SessionReadDTO
        {
            Id = session.Id,
            Title = session.Title,
            Description = session.Description,
            StartTime = session.StartTime,
            EndTime = session.EndTime,
            EventTitle = session.Event.Title,
            RoomName = session.Room.Name
        };
    }

    public async Task<SessionReadDTO?> GetSessionByIdAsync(int id)
    {
        var session = await _context.Sessions
            .Include(s => s.Event)
            .Include(s => s.Room)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (session == null) return null;

        return new SessionReadDTO
        {
            Id = session.Id,
            Title = session.Title,
            Description = session.Description,
            StartTime = session.StartTime,
            EndTime = session.EndTime,
            EventTitle = session.Event!.Title,
            RoomName = session.Room!.Name,
            Speakers = session.SessionSpeakers?.Select(ss => new SpeakerReadDTO
            {
                Id = ss.SpeakerId,
                FirstName = ss.Speaker!.FirstName,
                LastName = ss.Speaker.LastName,
                Bio = ss.Speaker.Bio,
                Role = ss.Role.ToString()             
            }).ToList() ?? new List<SpeakerReadDTO>(),
            Ratings = session.Ratings?.Select(r => new RatingReadDTO
            {
                Score = r.Score,
                Comment = r.Comment
            }).ToList() ?? new List<RatingReadDTO>()
        };
    }

    public async Task<IEnumerable<SessionReadDTO>> GetSessionsByEventAsync(int eventId)
    {
        return await _context.Sessions
            .Where(s => s.EventId == eventId)
            .Include(s => s.Room)
            .Select(s => new SessionReadDTO
            {
                Id = s.Id,
                Title = s.Title,
                Description = s.Description,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                EventTitle = s.Event!.Title,
                RoomName = s.Room!.Name
            })
            .ToListAsync();
    }

    public async Task<bool> UpdateSessionAsync(int id, SessionUpdateDTO dto)
    {
        var session = await _context.Sessions.FindAsync(id);
        if (session == null) return false;

        session.Title = dto.Title;
        session.Description = dto.Description;
        session.StartTime = dto.StartTime;
        session.EndTime = dto.EndTime;
        session.EventId = dto.EventId;
        session.RoomId = dto.RoomId;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteSessionAsync(int id)
    {
        var session = await _context.Sessions.FindAsync(id);
        if (session == null) return false;

        _context.Sessions.Remove(session);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<SessionReadDTO>> GetScheduleByEventAsync(int eventId)
    {
        return await _context.Sessions
            .Where(s => s.EventId == eventId)
            .OrderBy(s => s.StartTime)
            .Select(s => new SessionReadDTO
            {
                Id = s.Id,
                Title = s.Title,
                Description = s.Description,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                EventTitle = s.Event!.Title,
                RoomName = s.Room!.Name
            })
            .ToListAsync();
    }

    public async Task<bool> AssignSpeakersToSessionAsync(int sessionId, IEnumerable<int> speakerIds)
    {
        var session = await _context.Sessions
            .Include(s => s.SessionSpeakers)
            .FirstOrDefaultAsync(s => s.Id == sessionId);

        if (session == null) return false;

        var speakers = await _context.Speakers
            .Where(s => speakerIds.Contains(s.Id))
            .ToListAsync();

        // Ajoutez seulement les conférenciers qui ne sont pas déjà associés à la session
        foreach (var speaker in speakers)
        {
            if (!session.SessionSpeakers.Any(ss => ss.SpeakerId == speaker.Id))
            {
                session.SessionSpeakers.Add(new SessionSpeaker
                {
                    SessionId = sessionId,
                    SpeakerId = speaker.Id
                });
            }
        }

        await _context.SaveChangesAsync();
        return true;
    }
}
