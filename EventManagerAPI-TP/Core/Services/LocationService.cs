using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;

public class LocationService : ILocationService
{
    private readonly ApplicationDbContext _context;

    public LocationService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<LocationReadDTO>> GetLocationsAsync()
    {
        return await _context.Locations
            .Include(l => l.Rooms)
            .Include(l => l.Events)
            .Select(l => new LocationReadDTO
            {
                Id = l.Id,
                Name = l.Name,
                Address = l.Address,
                City = l.City,
                Country = l.Country,
                Capacity = l.Capacity,
                Rooms = l.Rooms.Select(r => new RoomReadDTO
                {
                    Id = r.Id,
                    Name = r.Name
                }).ToList(),
                Events = l.Events.Select(e => new EventReadDTO
                {
                    Id = e.Id,
                    Title = e.Title
                }).ToList()
            })
            .ToListAsync();
    }

    public async Task<LocationDTO?> GetLocationByIdAsync(int id)
    {
        var location = await _context.Locations
            .Include(l => l.Rooms)
            .Include(l => l.Events)
            .FirstOrDefaultAsync(l => l.Id == id);

        if (location == null) return null;

        return new LocationDTO
        {
            Id = location.Id,
            Name = location.Name,
            Address = location.Address,
            City = location.City,
            Country = location.Country,
            Capacity = location.Capacity,
            Rooms = location.Rooms.Select(r => new RoomReadDTO
            {
                Id = r.Id,
                Name = r.Name
            }).ToList(),
            Events = location.Events.Select(e => new EventReadDTO
            {
                Id = e.Id,
                Title = e.Title
            }).ToList()
        };
    }

    public async Task<LocationDTO> CreateLocationAsync(LocationCreateDTO locationCreateDTO)
    {
        var location = new Location
        {
            Name = locationCreateDTO.Name,
            Address = locationCreateDTO.Address,
            City = locationCreateDTO.City,
            Country = locationCreateDTO.Country,
            Capacity = locationCreateDTO.Capacity,
            ZipCode = locationCreateDTO.ZipCode
        };

        _context.Locations.Add(location);
        await _context.SaveChangesAsync();

        return new LocationDTO
        {
            Id = location.Id,
            Name = location.Name,
            Address = location.Address,
            City = location.City,
            Country = location.Country,
            Capacity = location.Capacity,
            ZipCode = location.ZipCode
        };
    }


    public async Task<LocationDTO?> UpdateLocationAsync(int id, LocationUpdateDTO locationUpdateDTO)
    {
        var location = await _context.Locations.FindAsync(id);
        if (location == null) return null;

        location.Name = locationUpdateDTO.Name;
        location.Address = locationUpdateDTO.Address;
        location.City = locationUpdateDTO.City;
        location.Country = locationUpdateDTO.Country;
        location.Capacity = locationUpdateDTO.Capacity;

        await _context.SaveChangesAsync();

        return new LocationDTO
        {
            Id = location.Id,
            Name = location.Name,
            Address = location.Address,
            City = location.City,
            Country = location.Country,
            Capacity = location.Capacity
        };
    }

    public async Task<bool> DeleteLocationAsync(int id)
    {
        var location = await _context.Locations.FindAsync(id);
        if (location == null) return false;

        _context.Locations.Remove(location);
        await _context.SaveChangesAsync();
        return true;
    }
}
