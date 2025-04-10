using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using EventManagerAPI_TP.Controllers;
using EventManagerAPI_TP.Core.DTO;
using EventManagerAPI_TP.Core.Interfaces;

namespace EventManagerAPI_TP.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;
        private readonly IEventListService _eventListService;

        public EventsController(IEventService eventService, IEventListService eventListService)
        {
            _eventService = eventService;
            _eventListService = eventListService;
        }

        [HttpPost]
        public async Task<ActionResult<Event>> CreateEvent(EventCreateDTO dto)
        {
            var createdEvent = await _eventService.CreateEventAsync(dto);
            return CreatedAtAction(nameof(GetEvent), new { id = createdEvent.Id }, createdEvent);
        }

        [HttpGet]
        public async Task<ActionResult<EventListResult>> GetEvents(
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] int? locationId,
            [FromQuery] int? category,
            [FromQuery] int? status,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var eventListResult = await _eventListService.GetEventsAsync(startDate, endDate, locationId, category, status, page, pageSize);

            return Ok(eventListResult);
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
}



