using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;

[Route("api/[controller]")]
[ApiController]
public class SpeakerController : ControllerBase
{
    private readonly ISpeakerService _speakerService;

    public SpeakerController(ISpeakerService speakerService)
    {
        _speakerService = speakerService;
    }

    [HttpPost("speakers")]
    public async Task<IActionResult> CreateSpeaker(SpeakerCreateDTO dto)
    {
        var speaker = await _speakerService.CreateSpeakerAsync(dto);
        return CreatedAtAction(nameof(GetSpeakerById), new { id = speaker.Id }, speaker);
    }

    [HttpGet("speakers/{id}")]
    public async Task<IActionResult> GetSpeakerById(int id)
    {
        var speaker = await _speakerService.GetSpeakerByIdAsync(id);

        if (speaker == null)
            return NotFound();

        return Ok(speaker);
    }

    [HttpGet("speakers")]
    public async Task<IActionResult> GetAllSpeakers()
    {
        var speakers = await _speakerService.GetAllSpeakersAsync();

        if (speakers == null || !speakers.Any())
            return NotFound();

        return Ok(speakers);
    }
}
