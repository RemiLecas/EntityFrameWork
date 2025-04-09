public interface IEventService
{
    Task<Event> CreateEventAsync(EventCreateDTO dto);
    Task<IEnumerable<EventReadDTO>> GetEventsAsync(DateTime? startDate, DateTime? endDate, int? locationId, int? category, int? status, int page, int pageSize);
    Task<EventReadDTO> GetEventByIdAsync(int id);
    Task<bool> UpdateEventAsync(int id, EventUpdateDTO eventUpdateDTO);
    Task<bool> DeleteEventAsync(int id);
}
