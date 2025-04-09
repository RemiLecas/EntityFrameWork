using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;

[Route("api/[controller]")]
[ApiController]
public class EventsController : ControllerBase
{
    private readonly IEventService _eventService;

    public EventsController(IEventService eventService, ICategoryService categoryService)
    {
        _eventService = eventService;
    }

    [HttpPost]
    public async Task<ActionResult<Event>> CreateEvent(EventCreateDTO dto)
    {
        var createdEvent = await _eventService.CreateEventAsync(dto);
        return CreatedAtAction(nameof(GetEvent), new { id = createdEvent.Id }, createdEvent);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EventReadDTO>>> GetEvents(
        [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, 
        [FromQuery] int? locationId, [FromQuery] int? category, 
        [FromQuery] int? status, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var events = await _eventService.GetEventsAsync(startDate, endDate, locationId, category, status, page, pageSize);
        return Ok(events);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EventReadDTO>> GetEvent(int id)
    {
        var eventDetails = await _eventService.GetEventByIdAsync(id);
        if (eventDetails == null)
        {
            return NotFound();
        }
        return Ok(eventDetails);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEvent(int id, EventUpdateDTO eventUpdateDTO)
    {
        var result = await _eventService.UpdateEventAsync(id, eventUpdateDTO);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEvent(int id)
    {
        var result = await _eventService.DeleteEventAsync(id);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }
}

