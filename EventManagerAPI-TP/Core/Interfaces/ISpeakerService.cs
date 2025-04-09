public interface ISpeakerService
{
    Task<SpeakerReadDTO> CreateSpeakerAsync(SpeakerCreateDTO dto);
    Task<SpeakerReadDTO?> GetSpeakerByIdAsync(int id);
    Task<IEnumerable<SpeakerReadDTO>> GetAllSpeakersAsync();
}
