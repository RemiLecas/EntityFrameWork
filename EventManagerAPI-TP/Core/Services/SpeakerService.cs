using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;

public class SpeakerService : ISpeakerService
{
    private readonly ApplicationDbContext _context;

    public SpeakerService(ApplicationDbContext context)
    {
        _context = context;
    }

    // Créer un conférencier
    public async Task<SpeakerReadDTO> CreateSpeakerAsync(SpeakerCreateDTO dto)
    {
        var speaker = new Speaker
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Bio = dto.Bio,
            Email = dto.Email,
            Company = dto.Company
        };

        _context.Speakers.Add(speaker);
        await _context.SaveChangesAsync();

        return new SpeakerReadDTO
        {
            Id = speaker.Id,
            FirstName = speaker.FirstName,
            LastName = speaker.LastName,
            Bio = speaker.Bio,
            Email = speaker.Email,
            Company = speaker.Company
        };
    }

    // Récupérer un conférencier par ID
    public async Task<SpeakerReadDTO?> GetSpeakerByIdAsync(int id)
    {
        var speaker = await _context.Speakers
            .FirstOrDefaultAsync(s => s.Id == id);

        if (speaker == null) return null;

        return new SpeakerReadDTO
        {
            Id = speaker.Id,
            FirstName = speaker.FirstName,
            LastName = speaker.LastName,
            Bio = speaker.Bio,
            Email = speaker.Email,
            Company = speaker.Company
        };
    }

    // Récupérer tous les conférenciers
    public async Task<IEnumerable<SpeakerReadDTO>> GetAllSpeakersAsync()
    {
        return await _context.Speakers
            .Select(s => new SpeakerReadDTO
            {
                Id = s.Id,
                FirstName = s.FirstName,
                LastName = s.LastName,
                Bio = s.Bio,
                Email = s.Email,
                Company = s.Company
            })
            .ToListAsync();
    }
}
