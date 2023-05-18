using Mapster;
using Pract.Dto;
using Pract.Models;
using Pract.Requests;

namespace Pract.Mappers
{
    public static class ChatRoomMapping
    {
        static ChatRoomMapping()
        {
            TypeAdapterConfig<ChatRoomRequest, ChatRoom>
                .NewConfig()
                .Map(x => x.Users, srs => new List<User>());
        }
        public static ChatRoomDto ToDto(this ChatRoom chatRoom)
        {
            return chatRoom.Adapt<ChatRoomDto>();
        }

        public static ChatRoom ToMoel(this ChatRoomRequest chatRoomRequest)
        {
            return chatRoomRequest.Adapt<ChatRoom>();
        }
    }
}
