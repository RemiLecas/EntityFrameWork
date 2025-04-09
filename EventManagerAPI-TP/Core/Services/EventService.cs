using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public class EventService : IEventService
{
    private readonly ApplicationDbContext _context;

    public EventService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Event> CreateEventAsync(EventCreateDTO dto)
    {
        // Vérification de l'existence de la location
        var location = await _context.Locations.FindAsync(dto.LocationId);
        if (location == null)
        {
            throw new ArgumentException("Location not found");
        }

        // Vérification de l'existence de la room
        var room = await _context.Rooms.FindAsync(dto.RoomId);
        if (room == null)
        {
            throw new ArgumentException("Room not found");
        }

        // Vérification de l'existence de la category
        var category = await _context.Categories.FindAsync(dto.CategoryId);
        if (category == null)
        {
            throw new ArgumentException("Category not found");
        }

        // Création de l'événement
        var @event = new Event
        {
            Title = dto.Title,
            Description = dto.Description,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            Status = (EventStatus)dto.Status,
            CategoryId = dto.CategoryId,
            LocationId = dto.LocationId,
            RoomId = dto.RoomId,
        };

        // Ajouter les participants à l'événement si la liste n'est pas vide
        if (dto.ParticipantIds != null && dto.ParticipantIds.Any())
        {
            var participants = await _context.Participants
                .Where(p => dto.ParticipantIds.Contains(p.Id))
                .ToListAsync();
            
            // Vérifier que tous les participants sont trouvés
            if (participants.Count != dto.ParticipantIds.Count)
            {
                throw new ArgumentException("One or more participant IDs not found");
            }

            foreach (var participant in participants)
            {
                @event.EventParticipants.Add(new EventParticipant
                {
                    EventId = @event.Id,
                    ParticipantId = participant.Id,
                    AttendanceStatus = AttendanceStatus.Pending // Par défaut, l'état est "Pending"
                });
            }
        }

        // Ajouter les sessions à l'événement si la liste n'est pas vide
        if (dto.SessionIds != null && dto.SessionIds.Any())
        {
            var sessions = await _context.Sessions
                .Where(s => dto.SessionIds.Contains(s.Id))
                .ToListAsync();
            
            // Vérifier que toutes les sessions sont trouvées
            if (sessions.Count != dto.SessionIds.Count)
            {
                throw new ArgumentException("One or more session IDs not found");
            }

            foreach (var session in sessions)
            {
                @event.Sessions.Add(session);

                if (dto.SpeakerIds != null && dto.SpeakerIds.Any())
                {
                    var speakers = await _context.Speakers
                        .Where(sp => dto.SpeakerIds.Contains(sp.Id))
                        .ToListAsync();
                    
                    // Vérifier que tous les speakers sont trouvés
                    if (speakers.Count != dto.SpeakerIds.Count)
                    {
                        throw new ArgumentException("One or more speaker IDs not found");
                    }

                    foreach (var speaker in speakers)
                    {
                        // Vérifier si la combinaison SessionId et SpeakerId existe déjà
                        var existingSessionSpeaker = await _context.SessionSpeakers
                            .FirstOrDefaultAsync(ss => ss.SessionId == session.Id && ss.SpeakerId == speaker.Id);

                        if (existingSessionSpeaker == null)
                        {
                            // Ajouter le speaker uniquement s'il n'existe pas déjà
                            session.SessionSpeakers.Add(new SessionSpeaker
                            {
                                SessionId = session.Id,
                                SpeakerId = speaker.Id,
                                Role = SpeakerRole.Speaker
                            });
                        }
                    }
                }
            }


        }

        // Ajouter l'événement dans le contexte et sauvegarder les changements
        _context.Events.Add(@event);
        await _context.SaveChangesAsync();

        return @event;
    }


    public async Task<EventReadDTO> GetEventByIdAsync(int id)
    {
        var @event = await _context.Events
            .Include(e => e.Location)
            .Include(e => e.Category)
            .Include(e => e.Sessions)
                .ThenInclude(s => s.Room)
            .Include(e => e.Sessions)
                .ThenInclude(s => s.SessionSpeakers)
                    .ThenInclude(ss => ss.Speaker)
            .Include(e => e.EventParticipants)
                .ThenInclude(ep => ep.Participant)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (@event == null)
            return null;

        // Création du DTO pour renvoyer toutes les informations
        return new EventReadDTO
        {
            Id = @event.Id,
            Title = @event.Title,
            Description = @event.Description,
            StartDate = @event.StartDate,
            EndDate = @event.EndDate,
            Status = (int)@event.Status,
            Category = @event.Category != null ? new CategoryReadDTO
            {
                Id = @event.Category.Id,
                Name = @event.Category.Name
            } : null,
            Location = @event.Location != null ? new LocationReadDTO
            {
                Id = @event.Location.Id,
                Name = @event.Location.Name,
                Address = @event.Location.Address,
                City = @event.Location.City,
                Country = @event.Location.Country,
                Capacity = @event.Location.Capacity
            } : null,
            Sessions = @event.Sessions?.Select(s => new SessionReadDTO
            {
                Id = s.Id,
                Title = s.Title,
                Description = s.Description,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                RoomName = s.Room?.Name ?? "Unknown",
                Speakers = s.SessionSpeakers?.Select(ss => new SpeakerReadDTO
                {
                    Id = ss.Speaker.Id,
                    FirstName = ss.Speaker.FirstName,
                    LastName = ss.Speaker.LastName,
                    Company = ss.Speaker.Company
                }).ToList() ?? new List<SpeakerReadDTO>()
            }).ToList() ?? new List<SessionReadDTO>(),
            Participants = @event.EventParticipants?.Select(ep => new ParticipantReadDTO
            {
                Id = ep.Participant.Id,
                FirstName = ep.Participant.FirstName,
                LastName = ep.Participant.LastName,
                Email = ep.Participant.Email
            }).ToList() ?? new List<ParticipantReadDTO>()
        };
    }


    public async Task<bool> UpdateEventAsync(int id, EventUpdateDTO eventUpdateDTO)
    {
        var existingEvent = await _context.Events.FindAsync(id);
        if (existingEvent == null)
        {
            return false;
        }

        existingEvent.Title = eventUpdateDTO.Title;
        existingEvent.Description = eventUpdateDTO.Description;
        existingEvent.StartDate = eventUpdateDTO.StartDate;
        existingEvent.EndDate = eventUpdateDTO.EndDate;
        existingEvent.Status = (EventStatus)eventUpdateDTO.Status;
        existingEvent.CategoryId = eventUpdateDTO.CategoryId;
        existingEvent.LocationId = eventUpdateDTO.LocationId;

        _context.Entry(existingEvent).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteEventAsync(int id)
    {
        var @event = await _context.Events.FindAsync(id);
        if (@event == null)
        {
            return false;
        }

        _context.Events.Remove(@event);
        await _context.SaveChangesAsync();

        return true;
    }
}

