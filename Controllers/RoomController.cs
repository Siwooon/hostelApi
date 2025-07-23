using HostelAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HostelAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : Controller
    {
        private IRoomRepository _roomRepository;
        public RoomController(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        [HttpGet("listAll")]
        public IActionResult ListAll()
        {
            var response = _roomRepository.GetAllRooms();
            return Ok(response);
        }
    }
}
