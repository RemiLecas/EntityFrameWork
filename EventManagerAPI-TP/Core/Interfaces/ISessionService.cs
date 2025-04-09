public interface ISessionService
{
    Task<SessionReadDTO?> GetAllSessions();
    Task<SessionReadDTO> CreateSessionAsync(SessionCreateDTO dto);
    Task<SessionReadDTO?> GetSessionByIdAsync(int id);
    Task<IEnumerable<SessionReadDTO>> GetSessionsByEventAsync(int eventId);
    Task<bool> UpdateSessionAsync(int id, SessionUpdateDTO dto);
    Task<bool> DeleteSessionAsync(int id);
    Task<IEnumerable<SessionReadDTO>> GetScheduleByEventAsync(int eventId);
    Task<bool> AssignSpeakersToSessionAsync(int sessionId, IEnumerable<int> speakerIds);
}
