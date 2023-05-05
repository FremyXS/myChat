using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pract.Database;
using Pract.Dto;
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

            return new UserDto(user);
        }

        public async Task<IEnumerable<UserDto>> GetUsers()
        {
            var users = _chatContext.Users.Select(el => new UserDto(el));


            return users;
        }

        public async Task<UserDto> CreateUser(UserRequest userRequest)
        {
            var user = await _chatContext.Users.FirstOrDefaultAsync(x => x.Login == userRequest.UserLogin);

            if(user != null)
            {
                throw new Exception("User is live");
            }

            var createUser = await _chatContext.Users.AddAsync(new User(userRequest));

            _chatContext.SaveChangesAsync();

            return new UserDto(createUser.Entity);
        }

        public async Task<UserDto> UpdateUser(long id, UserRequest userRequest)
        {
            var user = await _chatContext.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                throw new Exception("Incorrect login or id");
            }

           var updateUser = _chatContext.Users.Update(user.Update(userRequest));

            _chatContext.SaveChangesAsync();

            return new UserDto(updateUser.Entity);
        }

        public async Task<UserDto> DeleteUser(long id)
        {
            var user = await _chatContext.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                throw new Exception("User is not live");
            }

            var updateUser = _chatContext.Users.Update(user.Delete());

            _chatContext.SaveChangesAsync();

            return new UserDto(updateUser.Entity);

        }
    }
}
