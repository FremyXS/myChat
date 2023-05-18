namespace Pract.Models
{
    public class ChatRoom
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public ICollection<User>? Users { get; set; }
        public ICollection<ChatMessage>? ChatMessages { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeleteDate { get; set; } = null;

        public ChatRoom() { }

        public ChatRoom Update(string? title, IEnumerable<User>? users)
        {
            if(title is not null)
                Title = title;

            if(users is not null)
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
