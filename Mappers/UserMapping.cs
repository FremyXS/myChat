using Mapster;
using Pract.Dto;
using Pract.Models;
using Pract.Requests;

namespace Pract.Mappers
{
    public static class UserMapping
    {
        public static UserDto ToDto(this User user)
        {
            return user.Adapt<UserDto>();
        }

        public static User ToModel(this UserRequest user)
        {
            return user.Adapt<User>();
        }
    }
}
