public interface IParticipantsService
{
    Task<IEnumerable<ParticipantReadDTO>> GetAllParticipantsAsync();
    Task<ParticipantReadDTO?> GetParticipantByIdAsync(int id);
    Task<ParticipantReadDTO> CreateParticipantAsync(ParticipantCreateDTO dto);
    Task<bool> UpdateParticipantAsync(int id, ParticipantUpdateDTO dto);
    Task<bool> DeleteParticipantAsync(int id);
    Task<IEnumerable<EventParticipantsReadDTO>> GetEventHistoryByParticipantAsync(int participantId);
}
