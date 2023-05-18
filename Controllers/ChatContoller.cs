using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pract.Database;
using Pract.Models;
using Pract.Requests;
using Pract.Services;

namespace Pract.Controllers
{
    [Authorize]
    [Route("/chat")]
    public class ChatContoller : Controller
    {
        private readonly ChatRoomService _chatRoomService;
        public ChatContoller(ChatRoomService chatRoomService)
        {
            _chatRoomService = chatRoomService;
        }

        [HttpGet("user/{id:long}")]
        public async Task<IActionResult> GetByIdUser([FromRoute] long id)
        {
            try
            {
                var chatRoom = await _chatRoomService.GetChatRoomsByUserId(id);
                return Ok(chatRoom);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> Get([FromRoute] long id)
        {
            try
            {
                var chatRoom = await _chatRoomService.GetChatRoomById(id);
                return Ok(chatRoom);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ChatRoomRequest createChatRoom)
        {
            try
            {
                var chatRoom = await _chatRoomService.CreateChatRoom(createChatRoom);
                return Ok(chatRoom);
            }
            catch (Exception ex) 
            {
                return BadRequest(ex);
            }
        }

        [HttpPatch("{id:long}")]
        public async Task<IActionResult> Update([FromRoute] long id, [FromBody] ChatRoomRequest chatRoomRequest)
        {
            try
            {
                var chatRoom = await _chatRoomService.UpdateChatRoom(id, chatRoomRequest);
                return Ok(chatRoom);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete([FromRoute] long id)
        {
            try
            {
                var chatRoom = await _chatRoomService.DeleteChatRoom(id);
                return Ok(chatRoom);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
