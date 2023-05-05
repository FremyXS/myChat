using Microsoft.EntityFrameworkCore;
using Pract.Database;
using Pract.Dto;
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
            var chatRooms = await _chatContext.ChatRooms.FirstOrDefaultAsync(el => el.Id == chatRoomId);

            if(chatRooms == null)
            {
                throw new Exception("Chat room is not live");
            }

            return new ChatRoomDto(chatRooms);
        }

        public async Task<IEnumerable<ChatRoomDto>> GetChatRoomsByUserId(long userId)
        {
            var user = await _chatContext.Users.Include(el => el.ChatRooms)
                .FirstOrDefaultAsync(user => user.Id == userId);

            if (user == null)
            {
                throw new Exception("User is not live");
            }

            var chatRooms = user.ChatRooms.Select(chatRoom => new ChatRoomDto(chatRoom));

            return chatRooms;
        }

        public async Task<ChatRoomDto> CreateChatRoom(ChatRoomRequest chatRoomRequest)
        {
            var isUsersLive = chatRoomRequest.UsersId.All(
                el => (_chatContext.Users.FirstOrDefault(user => user.Id == el) != null)
            );

            if (!isUsersLive)
            {
                throw new Exception("One or many users are not live");
            }

            var users = chatRoomRequest.UsersId.Select(id =>
                _chatContext.Users.FirstOrDefaultAsync(user => user.Id == id).Result
            );

            var chatRoom = new ChatRoom(chatRoomRequest, users);

            var chatModel = await _chatContext.AddAsync(chatRoom);

            await _chatContext.SaveChangesAsync();

            return new ChatRoomDto(chatModel.Entity);
        }

        public async Task<ChatRoomDto> UpdateChatRoom(long chatRoomId, ChatRoomRequest chatRoomRequest)
        {
            var chatRooms = await _chatContext.ChatRooms.Include(x => x.Users)
                .FirstOrDefaultAsync(el => el.Id == chatRoomId);


            if (chatRooms == null)
            {
                throw new Exception("Chat room is not live");
            }

            var isUsersLive = chatRoomRequest.UsersId.All(
                el => (_chatContext.Users.FirstOrDefault(user => user.Id == el) != null)
            );

            if (!isUsersLive)
            {
                throw new Exception("One or many users are not live");
            }

            var users = chatRoomRequest.UsersId.Select(id =>
                 _chatContext.Users.FirstOrDefaultAsync(user => user.Id == id).Result
            );

            var updateChatRoom =  _chatContext.ChatRooms.Update(chatRooms.Update(chatRoomRequest, users));

            _chatContext.SaveChangesAsync();

            return new ChatRoomDto(updateChatRoom.Entity);
        }
        public async Task<ChatRoomDto> DeleteChatRoom(long id)
        {
            var chatRoom = await _chatContext.ChatRooms.FirstOrDefaultAsync(x => x.Id == id);

            if (chatRoom == null)
            {
                throw new Exception("Chat Room is not live");
            }

            var updateChatRoom = _chatContext.ChatRooms.Update(chatRoom.Delete());

            _chatContext.SaveChangesAsync();

            return new ChatRoomDto(updateChatRoom.Entity);

        }

    }
}
