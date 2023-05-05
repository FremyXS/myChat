using Pract.Requests;

namespace Pract.Models
{
    public class User
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public ICollection<ChatRoom>? ChatRooms { get; set; }
        public ICollection<ChatMessage>? ChatMessages{ get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeleteDate { get; set; }

        public User() { }

        public User(long id, string name, string login)
        {
            Id = id;
            Name = name;
            Login = login;
            IsDeleted = false;
            DeleteDate = null;
        }

        public User(UserRequest userRequest)
        {
            Name = userRequest.UserName;
            Login = userRequest.UserLogin;
        }

        public User Update(UserRequest userRequest)
        {
            if(userRequest.UserName is not null) Name = userRequest.UserName;
            if (userRequest.UserLogin is not null) Login = userRequest.UserLogin;

            return this;
        }

        public User Delete()
        {
            IsDeleted = true;
            DeleteDate = DateTime.UtcNow;

            return this;
        }
    }
}
