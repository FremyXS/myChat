using Microsoft.EntityFrameworkCore;
using Pract.Database;
using Pract.Dto;
using Pract.Mappers;
using Pract.Models;
using Pract.Requests;

namespace Pract.Services
{
    public class ChatRoomService
    {
        private readonly ChatContext _chatContext;
        public ChatRoomService(ChatContext chatContext)
        {
            _chatContext = chatContext;
        }

        public async Task<ChatRoomDto> GetChatRoomById(long chatRoomId)
        {
            var chatRoom = await _chatContext.ChatRooms.FirstOrDefaultAsync(el => el.Id == chatRoomId);

            if(chatRoom == null)
            {
                throw new Exception("Chat room is not live");
            }

            return chatRoom.ToDto();
        }

        public async Task<IEnumerable<ChatRoomDto>> GetChatRoomsByUserId(long userId)
        {
            var user = await _chatContext.Users.Include(el => el.ChatRooms)
                .FirstOrDefaultAsync(user => user.Id == userId);

            if (user == null)
            {
                throw new Exception("User is not live");
            }

            var chatRooms = user.ChatRooms.Select(chatRoom => chatRoom.ToDto());

            return chatRooms;
        }

        public async Task<ChatRoomDto> CreateChatRoom(ChatRoomRequest chatRoomRequest)
        {
            var allUsers = await _chatContext.Users.ToListAsync();

            var isUsersLive = chatRoomRequest.UsersId.All(
                el => (allUsers.FirstOrDefault(user => user.Id == el) != null)
            );

            if (!isUsersLive)
            {
                throw new Exception("One or many users are not live");
            }

            var users = chatRoomRequest.UsersId.Select(id =>
                allUsers.First(user => user.Id == id)
            );

            //var chatRoom = new ChatRoom(chatRoomRequest, users);

            var chatRoom = chatRoomRequest.ToMoel();

            chatRoom.Users = users.ToList();

            await _chatContext.AddAsync(chatRoom);

            await _chatContext.SaveChangesAsync();

            return chatRoom.ToDto();
        }

        public async Task<ChatRoomDto> UpdateChatRoom(long chatRoomId, ChatRoomRequest chatRoomRequest)
        {
            var chatRoom = await _chatContext.ChatRooms.Include(x => x.Users)
                .FirstOrDefaultAsync(el => el.Id == chatRoomId);


            if (chatRoom == null)
            {
                throw new Exception("Chat room is not live");
            }

            var allUsers = await _chatContext.Users.ToListAsync();

            var isUsersLive = chatRoomRequest.UsersId.All(
                el => (allUsers.FirstOrDefault(user => user.Id == el) != null)
            );

            if (!isUsersLive)
            {
                throw new Exception("One or many users are not live");
            }

            var users = chatRoomRequest.UsersId.Select(id =>
                 allUsers.First(user => user.Id == id)
            );

            chatRoom = chatRoom.Update(chatRoomRequest.Title, users);

            _chatContext.ChatRooms.Update(chatRoom);

            await _chatContext.SaveChangesAsync();

            return chatRoom.ToDto();
        }
        public async Task<ChatRoomDto> DeleteChatRoom(long id)
        {
            var chatRoom = await _chatContext.ChatRooms.FirstOrDefaultAsync(x => x.Id == id);

            if (chatRoom == null)
            {
                throw new Exception("Chat Room is not live");
            }

            chatRoom = chatRoom.Delete();

            var updateChatRoom = _chatContext.ChatRooms.Update(chatRoom);

            await _chatContext.SaveChangesAsync();

            return chatRoom.ToDto();

        }

    }
}
