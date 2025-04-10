using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class RoomService : IRoomService
{
    private readonly ApplicationDbContext _context;

    public RoomService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<RoomReadDTO>> GetRoomsAsync()
    {
        return await _context.Rooms
            .Include(r => r.Location)
            .Include(r => r.Sessions)
            .Select(r => new RoomReadDTO
            {
                Id = r.Id,
                Name = r.Name,
                Capacity = r.Capacity,
                Location = new LocationReadDTO
                {
                    Id = r.Location.Id,
                    Name = r.Location.Name,
                    Address = r.Location.Address,
                    City = r.Location.City,
                    Country = r.Location.Country,
                    Capacity = r.Location.Capacity
                },
                Sessions = r.Sessions.Select(s => new SessionReadDTO
                {
                    Id = s.Id,
                    Title = s.Title
                }).ToList()
            })
            .ToListAsync();
    }

    public async Task<RoomReadDTO?> GetRoomByIdAsync(int id)
    {
        var room = await _context.Rooms
            .Include(r => r.Location)
            .Include(r => r.Sessions)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (room == null) return null;

        return new RoomReadDTO
        {
            Id = room.Id,
            Name = room.Name,
            Capacity = room.Capacity,
            Location = new LocationReadDTO
            {
                Id = room.Location.Id,
                Name = room.Location.Name,
                Address = room.Location.Address,
                City = room.Location.City,
                Country = room.Location.Country,
                Capacity = room.Location.Capacity
            },
            Sessions = room.Sessions.Select(s => new SessionReadDTO
            {
                Id = s.Id,
                Title = s.Title
            }).ToList()
        };
    }

    public async Task<RoomReadDTO> CreateRoomAsync(RoomCreateDTO roomCreateDTO)
    {
        // Vérification si la location existe
        var locationExists = await _context.Locations.AnyAsync(l => l.Id == roomCreateDTO.LocationId);
        if (!locationExists)
            throw new ArgumentException("La location spécifiée n'existe pas.");

        // Créer la salle
        var room = new Room
        {
            Name = roomCreateDTO.Name,
            Capacity = roomCreateDTO.Capacity,
            LocationId = roomCreateDTO.LocationId
        };

        _context.Rooms.Add(room);
        await _context.SaveChangesAsync();

        // Retourne les informations de la salle
        return new RoomReadDTO
        {
            Id = room.Id,
            Name = room.Name,
            Capacity = room.Capacity,
            Location = new LocationReadDTO
            {
                Id = room.LocationId,
                Name = room.Location.Name,
                Address = room.Location.Address,
                City = room.Location.City,
                Country = room.Location.Country,
                Capacity = room.Location.Capacity
            },
            Sessions = room.Sessions?.Select(s => new SessionReadDTO
            {
                Id = s.Id,
                Title = s.Title
            }).ToList() ?? new List<SessionReadDTO>()
        };
    }

    public async Task<RoomUpdateDTO?> UpdateRoomAsync(int id, RoomUpdateDTO roomUpdateDTO)
    {
        var room = await _context.Rooms.FindAsync(id);
        if (room == null) return null;

        // Mettre à jour les informations de la salle
        room.Name = roomUpdateDTO.Name;
        room.Capacity = roomUpdateDTO.Capacity;

        await _context.SaveChangesAsync();

        return new RoomUpdateDTO
        {
            Name = room.Name,
            Capacity = room.Capacity
        };
    }

    public async Task<bool> DeleteRoomAsync(int id)
    {
        var room = await _context.Rooms.FindAsync(id);
        if (room == null) return false;

        _context.Rooms.Remove(room);
        await _context.SaveChangesAsync();
        return true;
    }
}
