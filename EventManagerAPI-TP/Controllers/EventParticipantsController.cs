using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;

[ApiController]
[Route("api/[controller]")]
public class EventParticipantsController : ControllerBase
{
    private readonly IEventParticipantsService _service;

    public EventParticipantsController(IEventParticipantsService service)
    {
        _service = service;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(EventParticipantsRegistrationDTO dto)
    {
        var result = await _service.RegisterToEventAsync(dto);
        return result ? Ok() : BadRequest("Registration failed.");
    }

    [HttpDelete("unregister")]
    public async Task<IActionResult> Unregister(int participantId, int eventId)
    {
        var result = await _service.UnregisterFromEventAsync(participantId, eventId);
        return result ? NoContent() : NotFound();
    }

    [HttpGet("event/{eventId}/participants")]
    public async Task<IActionResult> GetParticipantsByEvent(int eventId)
    {
        var participants = await _service.GetParticipantsByEventAsync(eventId);
        return Ok(participants);
    }
}
