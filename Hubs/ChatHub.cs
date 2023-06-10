using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Pract.Database;
using Pract.Dto;
using Pract.Mappers;
using Pract.Models;
using Pract.Requests;
using System.Security.Claims;

namespace Pract.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ChatContext _chatContext;
        public ChatHub(ChatContext chatContext)
        {
            _chatContext = chatContext;
        }
        //public async Task Send(string username, string message)
        //{
        //    await this.Clients.All.SendAsync("Receive", username, message);
        //}

        public async Task Send(ChatMessageRequest messageRequest)
        {

            var isUserLive = await _chatContext.Users.FirstOrDefaultAsync(user => user.Id == messageRequest.UserId);

            if (isUserLive == null)
            {
                throw new Exception("User is not live");
            }

            var isChatRoomLive = await _chatContext.ChatRooms.FirstOrDefaultAsync(chat => chat.Id == messageRequest.ChatRoomId);

            if (isChatRoomLive == null)
            {
                throw new Exception("Chat is not live");
            }

            var message = messageRequest.ToModel();

            await _chatContext.ChatMessages.AddAsync(message);

            await _chatContext.SaveChangesAsync();

            await this.Clients.All.SendAsync("Receive", messageRequest);
        }

        public async Task<IEnumerable<ChatMessageDto>> GetMessages(int chatId)
        {
            var chat = await _chatContext.ChatRooms.Include(el => el.ChatMessages)
                .Include(el => el.Users)
                .FirstOrDefaultAsync(x => x.Id == chatId); ;

            if (chat == null)
            {
                throw new Exception("Chat is not live");
            }

            var messages = chat.ChatMessages.Select(el => el.ToDto());

            return messages;
        }
    }
}
