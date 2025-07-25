using HostelAPI.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController, Route("[controller]")]
public class RoomController : ControllerBase
{
    private readonly IRoomService _service;

    public RoomController(IRoomService service)
    {
        _service = service;
    }

    [Authorize(Roles = "Client,Receptionist")]
    [HttpGet("availables")]
    public async Task<IActionResult> GetAvailableRooms([FromQuery] DateTime dateDebut, [FromQuery] DateTime dateFin)
    {
        var rooms = await _service.GetAvailablesAsync(dateDebut, dateFin);
        return Ok(rooms);
    }

    [Authorize(Roles = "Receptionist")]
    [HttpGet]
    public async Task<IActionResult> GetAllRooms()
    {
        var rooms = await _service.GetAllAsync();
        return Ok(rooms);
    }

    [Authorize(Roles = "Receptionist")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetRoom(int id)
    {
        var room = await _service.GetByIdAsync(id);
        return room == null ? NotFound() : Ok(room);
    }

    [Authorize(Roles = "Receptionist")]
    [HttpPost]
    public async Task<IActionResult> CreateRoom([FromBody] RoomDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetRoom), new { id = created }, created);
    }

    [Authorize(Roles = "Receptionist")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRoom(int id, [FromBody] RoomDto dto)
    {
        var updated = await _service.UpdateAsync(id, dto);
        return updated == null ? NotFound() : Ok(updated);
    }

    [Authorize(Roles = "Receptionist")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRoom(int id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }

    [Authorize(Roles = "Housekeeping")]
    [HttpGet("toclean")]
    public async Task<IActionResult> GetRoomsToClean()
    {
        var rooms = await _service.GetAllAsync();
        var toClean = rooms.Where(r => r.Status.Contains(RoomStatus.NeedsCleaning.ToString()));
        return Ok(toClean);
    }

    [Authorize(Roles = "Housekeeping")]
    [HttpPut("clean/{id}")]
    public async Task<IActionResult> CleanRoom(int id)
    {
        var room = await _service.GetByIdAsync(id);
        if (room == null)
            return NotFound();
        var list = room.Status.Split(',').ToList();
        list = list.Where(item => item != RoomStatus.NeedsCleaning.ToString()).ToList();
        if (list.Contains(RoomStatus.Available.ToString()))
        {
            list.Add(RoomStatus.Available.ToString());
        }

        room.Status = string.Join(',', list);
        var updated = await _service.UpdateAsync(id, room);
        return Ok(updated);
    }
}
