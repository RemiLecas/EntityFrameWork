public interface IEventParticipantsService
{
    Task<bool> RegisterToEventAsync(EventParticipantsRegistrationDTO dto);

    Task<bool> UnregisterFromEventAsync(int participantId, int eventId);

    Task<IEnumerable<ParticipantReadDTO>> GetParticipantsByEventAsync(int eventId);
}
