public interface ILocationService
{
    Task<IEnumerable<LocationReadDTO>> GetLocationsAsync();
    Task<LocationDTO?> GetLocationByIdAsync(int id);
    Task<LocationDTO> CreateLocationAsync(LocationCreateDTO locationCreateDTO);
    Task<LocationDTO?> UpdateLocationAsync(int id, LocationUpdateDTO locationUpdateDTO);
    Task<bool> DeleteLocationAsync(int id);
}
