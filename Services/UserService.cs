using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pract.Database;
using Pract.Dto;
using Pract.Mappers;
using Pract.Models;
using Pract.Requests;
using System.Linq;

namespace Pract.Services
{
    public class UserService
    {
        private readonly ChatContext _chatContext;
        public UserService(ChatContext chatContext)
        {
            _chatContext = chatContext;
        }

        public async Task<UserDto> GetUser(long id)
        {
            var user = await _chatContext.Users.FirstOrDefaultAsync(x => x.Id == id);

            if(user == null)
            {
                throw new Exception("User is not live");
            }

            return user.ToDto();
        }

        public async Task<IEnumerable<UserDto>> GetUsers()
        {
            var users = _chatContext.Users.Select(el => el.ToDto());


            return users;
        }

        //public async Task<UserDto> CreateUser(UserRequest userRequest)
        //{

        //    var createUser = await _chatContext.Users.AddAsync(new User(userRequest));

        //    _chatContext.SaveChangesAsync();

        //    return new UserDto(createUser.Entity);
        //}

        public async Task<UserDto> UpdateUser(long id, UserRequest userRequest)
        {
            var user = await _chatContext.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                throw new Exception("Incorrect login or id");
            }

            var newUser = userRequest.ToModel();

            user = user.Update(newUser);

           var updateUser = _chatContext.Users.Update(user);

            _chatContext.SaveChangesAsync();

            return user.ToDto();
        }

        //public async Task<UserDto> DeleteUser(long id)
        //{
        //    var user = await _chatContext.Users.FirstOrDefaultAsync(x => x.Id == id);

        //    if (user == null)
        //    {
        //        throw new Exception("User is not live");
        //    }

        //    var updateUser = _chatContext.Users.Update(user.Delete());

        //    _chatContext.SaveChangesAsync();

        //    return new UserDto(updateUser.Entity);

        //}
    }
}
