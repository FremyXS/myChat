using Microsoft.AspNetCore.Mvc;
using Pract.Requests;
using Pract.Services;

namespace Pract.Controllers
{
    [Route("/message")]
    public class ChatMessageController: Controller
    {
        private readonly ChatMessageService _chatMessageService;

        public ChatMessageController(ChatMessageService chatMessageService)
        {
            _chatMessageService = chatMessageService;
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById([FromRoute] long id)
        {
            try
            {
                var messageDto = await _chatMessageService.GetMessageById(id);
                return Ok(messageDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("chat/{id:long}")]
        public async Task<IActionResult> GetByChatId([FromRoute] long id)
        {
            try
            {
                var messageDto = await _chatMessageService.GetMessagesByChatId(id);
                return Ok(messageDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ChatMessageRequest message)
        {
            try
            {
                var messageDto =  await _chatMessageService.CreateMessage(message);
                return Ok(messageDto);
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
