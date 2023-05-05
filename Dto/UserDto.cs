using Pract.Models;

namespace Pract.Dto
{
    public class UserDto
    {
        public string UserName { get; set; }

        public UserDto(User user)
        {
            UserName = user.Name;
        }
    }
}
