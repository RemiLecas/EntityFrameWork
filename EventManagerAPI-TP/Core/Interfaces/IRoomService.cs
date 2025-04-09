public interface IRoomService
{
    Task<IEnumerable<RoomReadDTO>> GetRoomsAsync();
    Task<RoomReadDTO?> GetRoomByIdAsync(int id);
    Task<RoomReadDTO> CreateRoomAsync(RoomCreateDTO roomCreateDTO);
    Task<RoomUpdateDTO?> UpdateRoomAsync(int id, RoomUpdateDTO roomUpdateDTO);
    Task<bool> DeleteRoomAsync(int id);
}
