using System;
using System.Collections.Generic;

namespace BackendApi.Core.Models.Dtos
{
    public class MessageDto
    {
         public int Id { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }
        public string MainPhoto { get; set; }
        public int SenderId { get; set; }
        public string SenderUsername { get; set; }
        public string Content { get; set; }
        public List<MessageRecivedUserDto> RecipientUsers { get; set; }
        public DateTimeOffset? DateRead { get; set; }
        public DateTimeOffset MessageSent { get; set; } = DateTime.UtcNow;
        public bool SenderDeleted { get; set; }
        public bool RecipientDeleted { get; set; }
    }
}