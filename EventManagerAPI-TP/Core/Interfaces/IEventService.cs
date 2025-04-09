public interface IEventService
{
    Task<Event> CreateEventAsync(EventCreateDTO dto);
    Task<EventReadDTO> GetEventByIdAsync(int id);
    Task<bool> UpdateEventAsync(int id, EventUpdateDTO eventUpdateDTO);
    Task<bool> DeleteEventAsync(int id);
}
