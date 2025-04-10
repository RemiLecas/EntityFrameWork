using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;

[ApiController]
[Route("api/[controller]")]
public class ParticipantsController : ControllerBase
{
    private readonly IParticipantsService _participantService;

    public ParticipantsController(IParticipantsService participantService)
    {
        _participantService = participantService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var participants = await _participantService.GetAllParticipantsAsync();
        return Ok(participants);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var participant = await _participantService.GetParticipantByIdAsync(id);
        if (participant == null)
            return NotFound();

        return Ok(participant);
    }

    [HttpPost]
    public async Task<IActionResult> Create(ParticipantCreateDTO dto)
    {
        var created = await _participantService.CreateParticipantAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, ParticipantUpdateDTO dto)
    {
        var updated = await _participantService.UpdateParticipantAsync(id, dto);
        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _participantService.DeleteParticipantAsync(id);
        return deleted ? NoContent() : NotFound();
    }

    [HttpGet("{id}/events")]
    public async Task<IActionResult> GetEventHistory(int id)
    {
        var history = await _participantService.GetEventHistoryByParticipantAsync(id);
        return Ok(history);
    }
}
