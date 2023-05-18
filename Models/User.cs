using Pract.Requests;

namespace Pract.Models
{
    public class User
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public ICollection<ChatRoom>? ChatRooms { get; set; }
        public ICollection<ChatMessage>? ChatMessages{ get; set; }
        public long AccountId { get; set; }
        public Account? Account { get; set; }

        public User() { }

        public User(string name, Account account)
        {
            Name = name;
            Account = account;
        }

        public User Update(User userRequest)
        {
            if(userRequest.Name is not null) Name = userRequest.Name;

            return this;
        }
    }
}
