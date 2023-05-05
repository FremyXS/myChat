using Pract.Requests;

namespace Pract.Models
{
    public class ChatMessage
    {
        public long Id { get; set; }
        public string Message { get; set; }
        public long UserId { get; set; }
        public User? User { get; set; }
        public long ChatRoomId { get; set; }
        public ChatRoom? ChatRoom { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeleteDate { get; set; } = null;

        public ChatMessage() { }

        public ChatMessage(ChatMessageRequest chatMessageRequest)
        {
            Message = chatMessageRequest.Message;
            UserId = chatMessageRequest.UserId;
            ChatRoomId = chatMessageRequest.ChatRoomId;

        }
    }
}
