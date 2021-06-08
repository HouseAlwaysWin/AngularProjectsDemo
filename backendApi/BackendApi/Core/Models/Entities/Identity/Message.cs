using System;
using System.Collections.Generic;
using BackendApi.Core.Entities.Identity;
using BackendApi.Core.Models.Entities.Identity;

namespace BackendApi.Core.Models.Entities
{
    public class Message: IBaseEntity 
    {
       public int Id { get; set; }
       public DateTimeOffset CreatedDate { get; set; }
       public DateTimeOffset? ModifiedDate { get; set; }
        public int SenderId { get; set; }
        public string SenderUsername { get; set; }
        public AppUser Sender { get; set; }
        // public int RecipientId { get; set; }
        // public string RecipientUsername { get; set; }
        // public AppUser Recipient { get; set; }
        public ICollection<MessageRecivedUser> RecipientUsers { get; set; }
        public string Content { get; set; }
        // public DateTimeOffset? DateRead { get; set; }
        public DateTimeOffset MessageSent { get; set; } = DateTime.UtcNow;
        public bool SenderDeleted { get; set; }
        public bool RecipientDeleted { get; set; }
        public int MessageGroupId { get; set; }
    }
}