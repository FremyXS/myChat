using Pract.Models;

namespace Pract.Dto
{
    public class ChatMessageDto
    {
        public string Message { get; set; }
        public long UserId { get; set; }
        public ChatMessageDto() { }
        public ChatMessageDto(ChatMessage message)
        {
            Message = message.Message;
            UserId = message.UserId;
        }
    }
}
