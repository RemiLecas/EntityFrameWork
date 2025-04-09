using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class DatabaseSeeder
{
    private readonly ApplicationDbContext _context;

    public DatabaseSeeder(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        Console.WriteLine("Début du seeding...");

        if (_context.Categories.Any())
        {
            Console.WriteLine("Les catégories existent déjà.");
            return;
        }

        // Ajouter des catégories
        var categories = new List<Category>
        {
            new Category { Name = "Technology" },
            new Category { Name = "Business" },
            new Category { Name = "Health" }
        };
        await _context.Categories.AddRangeAsync(categories);
        await _context.SaveChangesAsync();

        if (_context.Locations.Any())
        {
            Console.WriteLine("Les Locations existent déjà.");
            return;
        }

        // Ajouter des locations
        var locations = new List<Location>
        {
            new Location { Name = "Conference Center A", Address = "123 Main St", City = "New York", Country = "USA", Capacity = 200, ZipCode = "10001" },
            new Location { Name = "Conference Center B", Address = "456 Broadway", City = "San Francisco", Country = "USA", Capacity = 150, ZipCode = "94105" }
        };
        await _context.Locations.AddRangeAsync(locations);
        await _context.SaveChangesAsync();

        if (_context.Events.Any())
        {
            Console.WriteLine("Les Events existent déjà.");
            return;
        }

        // Ajouter des événements
        var events = new List<Event>
        {
            new Event
            {
                Title = "Tech Summit 2025",
                Description = "An event to explore the latest in technology.",
                StartDate = DateTime.Now.AddMonths(1),
                EndDate = DateTime.Now.AddMonths(1).AddDays(2),
                CategoryId = categories[0].Id,
                LocationId = locations[0].Id,
            },
            new Event
            {
                Title = "Business Leadership Conference",
                Description = "A conference for business leaders and innovators.",
                StartDate = DateTime.Now.AddMonths(2),
                EndDate = DateTime.Now.AddMonths(2).AddDays(3),
                CategoryId = categories[1].Id,
                LocationId = locations[1].Id,
            }
        };
        await _context.Events.AddRangeAsync(events);
        await _context.SaveChangesAsync();

        if (_context.Participants.Any())
        {
            Console.WriteLine("Les Participants existent déjà.");
            return;
        }

        // Ajouter des participants
        var participants = new List<Participant>
        {
            new Participant { FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" },
            new Participant { FirstName = "Jane", LastName = "Smith", Email = "jane.smith@example.com" }
        };
        await _context.Participants.AddRangeAsync(participants);
        await _context.SaveChangesAsync();

        if (_context.Rooms.Any())
        {
            Console.WriteLine("Les Rooms existent déjà.");
            return;
        }

        // Ajouter des salles
        var rooms = new List<Room>
        {
            new Room { Name = "Room A", Capacity = 100, LocationId = locations[0].Id },
            new Room { Name = "Room B", Capacity = 50, LocationId = locations[1].Id }
        };
        await _context.Rooms.AddRangeAsync(rooms);
        await _context.SaveChangesAsync();

        if (_context.Sessions.Any())
        {
            Console.WriteLine("Les Sessions existent déjà.");
            return;
        }

        // Ajouter des sessions
        var sessions = new List<Session>
        {
            new Session { Title = "Keynote: The Future of Tech", StartTime = DateTime.Now.AddMonths(1).AddDays(1), EndTime = DateTime.Now.AddMonths(1).AddDays(1).AddHours(2), EventId = events[0].Id, RoomId = rooms[0].Id },
            new Session { Title = "Panel: Business Growth in 2025", StartTime = DateTime.Now.AddMonths(2).AddDays(2), EndTime = DateTime.Now.AddMonths(2).AddDays(2).AddHours(1), EventId = events[1].Id, RoomId = rooms[1].Id }
        };
        await _context.Sessions.AddRangeAsync(sessions);
        await _context.SaveChangesAsync();

        if (_context.Speakers.Any())
        {
            Console.WriteLine("Les Speakers existent déjà.");
            return;
        }

        // Ajouter des intervenants (speakers)
        var speakers = new List<Speaker>
        {
            new Speaker { FirstName = "Alice", LastName = "Johnson", Email = "alice.johnson@example.com" },
            new Speaker { FirstName = "Bob", LastName = "Brown", Email = "bob.brown@example.com" }
        };
        await _context.Speakers.AddRangeAsync(speakers);
        await _context.SaveChangesAsync();

        if (_context.SessionSpeakers.Any())
        {
            Console.WriteLine("Les SessionSpeakers existent déjà.");
            return;
        }

        // Ajouter des relations Speaker-Session
        var sessionSpeakers = new List<SessionSpeaker>
        {
            new SessionSpeaker { SessionId = sessions[0].Id, SpeakerId = speakers[0].Id, Role = SpeakerRole.Keynote.ToString() },
            new SessionSpeaker { SessionId = sessions[1].Id, SpeakerId = speakers[1].Id, Role = SpeakerRole.Panelist.ToString() }
        };
        await _context.SessionSpeakers.AddRangeAsync(sessionSpeakers);
        await _context.SaveChangesAsync();

        if (_context.Ratings.Any())
        {
            Console.WriteLine("Les Ratings existent déjà.");
            return;
        }

        // Ajouter des évaluations (ratings)
        var ratings = new List<Rating>
        {
            new Rating { SessionId = sessions[0].Id, ParticipantId = participants[0].Id, Score = 5, Comment = "Excellent talk!" },
            new Rating { SessionId = sessions[1].Id, ParticipantId = participants[1].Id, Score = 4, Comment = "Very informative." }
        };
        await _context.Ratings.AddRangeAsync(ratings);
        await _context.SaveChangesAsync();

        if (_context.EventParticipants.Any())
        {
            Console.WriteLine("Les EventParticipants existent déjà.");
            return;
        }

        // Ajouter des inscriptions de participants à des événements
        var eventParticipants = new List<EventParticipant>
        {
            new EventParticipant { EventId = events[0].Id, ParticipantId = participants[0].Id, AttendanceStatus = AttendanceStatus.Confirmed },
            new EventParticipant { EventId = events[1].Id, ParticipantId = participants[1].Id, AttendanceStatus = AttendanceStatus.Pending }
        };
        await _context.EventParticipants.AddRangeAsync(eventParticipants);
        await _context.SaveChangesAsync();

        Console.WriteLine("Seeding terminé.");
    }
}
