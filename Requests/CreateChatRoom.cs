namespace Pract.Requests
{
    public class ChatRoomRequest
    {
        public string Title { get; set; }
        public IEnumerable<long>? UsersId { get; set; }
    }
}
