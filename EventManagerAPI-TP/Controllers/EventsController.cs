using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;

namespace EventManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EventsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // CREATE: api/events
        [HttpPost]
        public async Task<ActionResult<Event>> CreateEvent(EventCreateDTO dto)
        {
            var location = await _context.Locations.FindAsync(dto.LocationId);

            if (location == null)
            {
                return NotFound("Location not found");
            }

            var @event = new Event
            {
                Title = dto.Title,
                Description = dto.Description,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Status = (EventStatus)dto.Status,
                Category = (EventCategory)dto.Category,
                LocationId = dto.LocationId,
                Location = location
            };

            _context.Events.Add(@event);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetEvent), new { id = @event.Id }, @event);
        }

        // READ: api/events
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetEvents()
        {
            var events = await _context.Events
                .Include(e => e.Location)
                .Include(e => e.Sessions)
                    .ThenInclude(s => s.Room)
                .Include(e => e.Sessions)
                    .ThenInclude(s => s.SessionSpeakers)
                        .ThenInclude(ss => ss.Speaker)
                .Include(e => e.EventParticipants)
                    .ThenInclude(ep => ep.Participant)
                .Select(e => new EventReadDTO
                {
                    Id = e.Id,
                    Title = e.Title,
                    Description = e.Description,
                    StartDate = e.StartDate,
                    EndDate = e.EndDate,
                    LocationName = e.Location != null ? e.Location.Name : "",

                    Sessions = e.Sessions.Select(s => new SessionDTO
                    {
                        Id = s.Id,
                        Title = s.Title,
                        Description = s.Description,
                        StartTime = s.StartTime,
                        EndTime = s.EndTime,
                        RoomName = s.Room != null ? s.Room.Name : "",
                        Speakers = s.SessionSpeakers.Select(ss => new SpeakerDTO
                        {
                            Id = ss.Speaker.Id,
                            FirstName = ss.Speaker.FirstName,
                            LastName = ss.Speaker.LastName,
                            Company = ss.Speaker.Company
                        }).ToList()
                    }).ToList(),

                    Participants = e.EventParticipants.Select(ep => new ParticipantReadDTO
                    {
                        Id = ep.Participant.Id,
                        FirstName = ep.Participant.FirstName,
                        LastName = ep.Participant.LastName,                        
                        Email = ep.Participant.Email
                    }).ToList()
                })
                .ToListAsync();

            return Ok(events);
        }
        // READ: api/events/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<EventReadDTO>> GetEvent(int id)
        {
            var @event = await _context.Events
                .Include(e => e.Location)
                .Include(e => e.Sessions)
                .Include(e => e.EventParticipants)
                .ThenInclude(ep => ep.Participant)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (@event == null)
            {
                return NotFound();
            }

            var eventReadDTO = new EventReadDTO
            {
                Id = @event.Id,
                Title = @event.Title,
                Description = @event.Description,
                StartDate = @event.StartDate,
                EndDate = @event.EndDate,
                LocationName = @event.Location?.Name,
                Sessions = @event.Sessions.Select(s => new SessionDTO
                {
                    Id = s.Id,
                    Title = s.Title,
                    Description = s.Description,
                    StartTime = s.StartTime,
                    EndTime = s.EndTime,
                    RoomName = s.Room?.Name,
                    Speakers = s.SessionSpeakers.Select(ss => new SpeakerDTO
                    {
                        Id = ss.Speaker.Id,
                        FirstName = ss.Speaker.FirstName,
                        LastName = ss.Speaker.LastName,
                        Bio = ss.Speaker.Bio
                    }).ToList()
                }).ToList(),
                Participants = @event.EventParticipants.Select(ep => new ParticipantReadDTO
                {
                    Id = ep.Participant.Id,
                    FirstName = ep.Participant.FirstName,
                    LastName = ep.Participant.LastName,
                    Email = ep.Participant.Email
                }).ToList()
            };

            return Ok(eventReadDTO);
        }


        // UPDATE: api/events/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(int id, EventUpdateDTO eventUpdateDTO)
        {
            var existingEvent = await _context.Events.FindAsync(id);
            if (existingEvent == null)
            {
                return NotFound();
            }

            existingEvent.Title = eventUpdateDTO.Title;
            existingEvent.Description = eventUpdateDTO.Description;
            existingEvent.StartDate = eventUpdateDTO.StartDate;
            existingEvent.EndDate = eventUpdateDTO.EndDate;
            existingEvent.Status = (EventStatus)eventUpdateDTO.Status;
            existingEvent.Category = (EventCategory)eventUpdateDTO.Category;
            existingEvent.LocationId = eventUpdateDTO.LocationId;

            _context.Entry(existingEvent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(id))
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


        // DELETE: api/events/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }

            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }
    }
}
