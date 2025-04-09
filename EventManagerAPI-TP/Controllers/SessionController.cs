using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;

[Route("api/[controller]")]
[ApiController]
public class SessionsController : ControllerBase
{
    private readonly ISessionService _sessionService;

    public SessionsController(ISessionService sessionService)
    {
        _sessionService = sessionService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateSession(SessionCreateDTO dto)
    {
        var session = await _sessionService.CreateSessionAsync(dto);
        return CreatedAtAction(nameof(GetSessionById), new { id = session.Id }, session);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllSessions()
    {
        var session = await _sessionService.GetAllSessions();
        if (session == null)
            return NotFound();

        return Ok(session);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSessionById(int id)
    {
        var session = await _sessionService.GetSessionByIdAsync(id);
        if (session == null)
            return NotFound();

        return Ok(session);
    }

    [HttpGet("event/{eventId}")]
    public async Task<IActionResult> GetSessionsByEvent(int eventId)
    {
        var sessions = await _sessionService.GetSessionsByEventAsync(eventId);
        return Ok(sessions);
    }

    [HttpGet("schedule/event/{eventId}")]
    public async Task<IActionResult> GetScheduleByEvent(int eventId)
    {
        var schedule = await _sessionService.GetScheduleByEventAsync(eventId);
        return Ok(schedule);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSession(int id, SessionUpdateDTO dto)
    {
        var updated = await _sessionService.UpdateSessionAsync(id, dto);
        if (!updated)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSession(int id)
    {
        var deleted = await _sessionService.DeleteSessionAsync(id);
        if (!deleted)
            return NotFound();

        return NoContent();
    }

    [HttpPost("{sessionId}/speakers")]
    public async Task<IActionResult> AssignSpeakersToSession(int sessionId, [FromBody] IEnumerable<int> speakerIds)
    {
        var assigned = await _sessionService.AssignSpeakersToSessionAsync(sessionId, speakerIds);
        if (!assigned)
            return NotFound();

        return NoContent();
    }
}
