using Pract.Models;

namespace Pract.Dto
{
    public class ChatMessageDto
    {
        public string Message { get; set; }
        public long UserId { get; set; }
        public string Name { get; set; }
    }
}
