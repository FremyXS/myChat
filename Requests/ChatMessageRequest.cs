namespace Pract.Requests
{
    public class ChatMessageRequest
    {
        public string Message { get; set; }
        public long UserId { get; set; }
        public long ChatRoomId { get; set; }
    }
}
