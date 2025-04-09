using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public class ParticipantService : IParticipantsService
{
    private readonly ApplicationDbContext _context;

    public ParticipantService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ParticipantReadDTO>> GetAllParticipantsAsync()
    {
        return await _context.Participants
            .Select(p => new ParticipantReadDTO
            {
                Id = p.Id,
                FirstName = p.FirstName,
                LastName = p.LastName,
                Email = p.Email,
                Company = p.Company,
                JobTitle = p.JobTitle
            })
            .ToListAsync();
    }

    public async Task<ParticipantReadDTO?> GetParticipantByIdAsync(int id)
    {
        var p = await _context.Participants.FindAsync(id);
        if (p == null) return null;

        return new ParticipantReadDTO
        {
            Id = p.Id,
            FirstName = p.FirstName,
            LastName = p.LastName,
            Email = p.Email,
            Company = p.Company,
            JobTitle = p.JobTitle
        };
    }

    public async Task<ParticipantReadDTO> CreateParticipantAsync(ParticipantCreateDTO dto)
    {
        var participant = new Participant
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            Company = dto.Company,
            JobTitle = dto.JobTitle
        };

        _context.Participants.Add(participant);
        await _context.SaveChangesAsync();

        return new ParticipantReadDTO
        {
            Id = participant.Id,
            FirstName = participant.FirstName,
            LastName = participant.LastName,
            Email = participant.Email,
            Company = participant.Company,
            JobTitle = participant.JobTitle
        };
    }

    public async Task<bool> UpdateParticipantAsync(int id, ParticipantUpdateDTO dto)
    {
        var participant = await _context.Participants.FindAsync(id);
        if (participant == null)
            return false;

        participant.FirstName = dto.FirstName;
        participant.LastName = dto.LastName;
        participant.Email = dto.Email;
        participant.Company = dto.Company;
        participant.JobTitle = dto.JobTitle;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteParticipantAsync(int id)
    {
        var participant = await _context.Participants.FindAsync(id);
        if (participant == null)
            return false;

        _context.Participants.Remove(participant);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<EventParticipantsReadDTO>> GetEventHistoryByParticipantAsync(int participantId)
    {
        return await _context.EventParticipants
            .Where(ep => ep.ParticipantId == participantId)
            .Include(ep => ep.Event)
            .Select(ep => new EventParticipantsReadDTO
            {
                Id = ep.Event!.Id,
                Title = ep.Event.Title,
                StartDate = ep.Event.StartDate,
                EndDate = ep.Event.EndDate,
                AttendanceStatus = ep.AttendanceStatus
            })
            .ToListAsync();
    }
}
