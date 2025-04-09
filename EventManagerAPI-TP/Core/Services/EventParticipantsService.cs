using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public class EventParticipantsService : IEventParticipantsService
{
    private readonly ApplicationDbContext _context;

    public EventParticipantsService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> RegisterToEventAsync(EventParticipantsRegistrationDTO dto)
    {
        var alreadyRegistered = await _context.EventParticipants
            .AnyAsync(ep => ep.ParticipantId == dto.ParticipantId && ep.EventId == dto.EventId);

        if (alreadyRegistered)
            return false;

        var registration = new EventParticipant
        {
            ParticipantId = dto.ParticipantId,
            EventId = dto.EventId,
            RegistrationDate = DateTime.UtcNow,
            AttendanceStatus = AttendanceStatus.Pending
        };

        _context.EventParticipants.Add(registration);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UnregisterFromEventAsync(int participantId, int eventId)
    {
        var record = await _context.EventParticipants
            .FirstOrDefaultAsync(ep => ep.ParticipantId == participantId && ep.EventId == eventId);

        if (record == null)
            return false;

        _context.EventParticipants.Remove(record);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<ParticipantReadDTO>> GetParticipantsByEventAsync(int eventId)
    {
        return await _context.EventParticipants
            .Where(ep => ep.EventId == eventId)
            .Include(ep => ep.Participant)
            .Select(ep => new ParticipantReadDTO
            {
                Id = ep.Participant!.Id,
                FirstName = ep.Participant.FirstName,
                LastName = ep.Participant.LastName,
                Email = ep.Participant.Email,
                Company = ep.Participant.Company,
                JobTitle = ep.Participant.JobTitle
            })
            .ToListAsync();
    }
}
