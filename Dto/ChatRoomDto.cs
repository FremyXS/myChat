﻿using Pract.Models;

namespace Pract.Dto
{
    public class ChatRoomDto
    {
        public string Title { get; set; }

        public ChatRoomDto()
        {

        }

        public ChatRoomDto(ChatRoom chatRoom)
        {
            Title = chatRoom.Title;
        }
    }
}
