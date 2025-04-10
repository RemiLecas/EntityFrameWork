using EventManagerAPI_TP.Core.DTO;

namespace EventManagerAPI_TP.Core.Interfaces {
    public interface IEventService
    {
        Task<Event> CreateEventAsync(EventCreateDTO dto);
        Task<EventReadDTO> GetEventByIdAsync(int id);
        Task<bool> UpdateEventAsync(int id, EventUpdateDTO eventUpdateDTO);
        Task<bool> DeleteEventAsync(int id);
    }
}

