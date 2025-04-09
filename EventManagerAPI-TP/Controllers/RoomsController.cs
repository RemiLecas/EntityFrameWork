using Microsoft.AspNetCore.Mvc;
using Infrastructure.Data;
using System.Threading.Tasks;
using System.Collections.Generic;

[Route("api/[controller]")]
[ApiController]
public class RoomsController : ControllerBase
{
    private readonly IRoomService _roomService;

    public RoomsController(IRoomService roomService)
    {
        _roomService = roomService;
    }

    [HttpGet]
    public async Task<IActionResult> GetRooms()
    {
        var rooms = await _roomService.GetRoomsAsync();
        return Ok(rooms);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetRoom(int id)
    {
        var room = await _roomService.GetRoomByIdAsync(id);
        if (room == null)
        {
            return NotFound();
        }
        return Ok(room);
    }

    [HttpPost]
    public async Task<IActionResult> CreateRoomAsync(RoomCreateDTO roomCreateDTO)
    {
        try
        {
            var roomDTO = await _roomService.CreateRoomAsync(roomCreateDTO);
            return CreatedAtAction(nameof(GetRoom), new { id = roomDTO.Id }, roomDTO);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRoomAsync(int id, RoomUpdateDTO roomUpdateDTO)
    {
        var updatedRoom = await _roomService.UpdateRoomAsync(id, roomUpdateDTO);
        if (updatedRoom == null)
        {
            return NotFound();
        }

        return Ok(updatedRoom);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRoom(int id)
    {
        var success = await _roomService.DeleteRoomAsync(id);
        if (!success)
        {
            return NotFound();
        }
        return NoContent();
    }
}
