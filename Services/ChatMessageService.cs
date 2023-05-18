using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pract.Database;
using Pract.Dto;
using Pract.Mappers;
using Pract.Models;
using Pract.Requests;

namespace Pract.Services
{
    public class ChatMessageService
    {
        private readonly ChatContext _chatContext;

        public ChatMessageService(ChatContext chatContext)
        {
            _chatContext = chatContext;
        }

        public async Task<ChatMessageDto> GetMessageById(long id)
        {
            var message = await _chatContext.ChatMessages.FirstOrDefaultAsync(x => x.Id == id);

            if(message == null)
            {
                throw new Exception("Message is not live");
            }

            return message.ToDto();
        }

        public async Task<IEnumerable<ChatMessageDto>> GetMessagesByChatId(long chatId)
        {
            var chat = await _chatContext.ChatRooms.Include(el => el.ChatMessages)
                .Include(el => el.Users)
                .FirstOrDefaultAsync(x => x.Id == chatId);

            if (chat == null)
            {
                throw new Exception("Chat is not live");
            }

            var messages = chat.ChatMessages.Select(el => el.ToDto());

            return messages;
        }

        public async Task<ChatMessageDto> CreateMessage(ChatMessageRequest messageRequest)
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

            return message.ToDto();
        }

    }
}
