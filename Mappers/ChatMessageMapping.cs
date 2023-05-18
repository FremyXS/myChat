using Mapster;
using Pract.Dto;
using Pract.Models;
using Pract.Requests;

namespace Pract.Mappers
{
    public static class ChatMessageMapping
    {
        static ChatMessageMapping()
        {
            TypeAdapterConfig<ChatMessage, ChatMessageDto>
                .NewConfig()
                .Map(x => x.Name, src => src.User.Name);
        }
        public static ChatMessageDto ToDto(this ChatMessage chatMessage)
        {
            return chatMessage.Adapt<ChatMessageDto>();
        }
        public static ChatMessage ToModel(this ChatMessageRequest chatRoomRequest)
        {
            return chatRoomRequest.Adapt<ChatMessage>();
        }
    }
}
