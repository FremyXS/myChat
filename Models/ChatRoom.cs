using Microsoft.EntityFrameworkCore.Update.Internal;
using Pract.Requests;

namespace Pract.Models
{
    public class ChatRoom
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public ICollection<User> Users { get; set; }
        public ICollection<ChatMessage>? ChatMessages { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeleteDate { get; set; } = null;

        public ChatRoom() { }

        public ChatRoom(ChatRoomRequest chatRoomRequest, IEnumerable<User> users) 
        {
            Title = chatRoomRequest.Title;
            Users = users.ToList();
        }

        public ChatRoom Update(ChatRoomRequest chatRoomRequest, IEnumerable<User> users)
        {
            Title = chatRoomRequest.Title;
            Users = users.ToList();

            return this;
        }

        public ChatRoom Delete()
        {
            IsDeleted = true;
            DeleteDate = DateTime.UtcNow;

            return this;
        }
    }
}
