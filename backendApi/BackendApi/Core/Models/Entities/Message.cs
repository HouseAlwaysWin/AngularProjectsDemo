using System;
using BackendApi.Core.Entities.Identity;

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
        public int RecipientId { get; set; }
        public string RecipientUsername { get; set; }
        public AppUser Recipient { get; set; }
        public string Content { get; set; }
        public DateTime? DateRead { get; set; }
        public DateTime MessageSent { get; set; } = DateTime.UtcNow;
        public bool SenderDeleted { get; set; }
        public bool RecipientDeleted { get; set; }
    }
}