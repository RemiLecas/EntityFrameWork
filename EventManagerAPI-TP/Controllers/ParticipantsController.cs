using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;

namespace EventManagerAPI.Controllers;

[Route("api/participants")]
[ApiController]
public class ParticipantsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ParticipantsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<ActionResult<Participant>> CreateParticipant(ParticipantCreateDTO participantDTO)
    {
        var participant = new Participant
        {
            FirstName = participantDTO.FirstName,
            LastName = participantDTO.LastName,
            Email = participantDTO.Email,
            Company = participantDTO.Company,
            JobTitle = participantDTO.JobTitle
        };

        _context.Participants.Add(participant);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetParticipant), new { id = participant.Id }, participant);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Participant>>> GetParticipants()
    {
        var participants = await _context.Participants.ToListAsync();
        return Ok(participants);
    }

    // Récupérer un participant par ID
    [HttpGet("{id}")]
    public async Task<ActionResult<Participant>> GetParticipant(int id)
    {
        var participant = await _context.Participants.FindAsync(id);

        if (participant == null)
        {
            return NotFound();
        }

        return Ok(participant);
    }

    // Mettre à jour un participant
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateParticipant(int id, ParticipantUpdateDTO participantDTO)
    {
        var participant = await _context.Participants.FindAsync(id);

        if (participant == null)
        {
            return NotFound();
        }

        participant.FirstName = participantDTO.FirstName;
        participant.LastName = participantDTO.LastName;
        participant.Email = participantDTO.Email;
        participant.Company = participantDTO.Company;
        participant.JobTitle = participantDTO.JobTitle;

        _context.Entry(participant).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ParticipantExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // Supprimer un participant
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteParticipant(int id)
    {
        var participant = await _context.Participants.FindAsync(id);

        if (participant == null)
        {
            return NotFound();
        }

        _context.Participants.Remove(participant);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // Vérifier si un participant existe
    private bool ParticipantExists(int id)
    {
        return _context.Participants.Any(e => e.Id == id);
    }

}