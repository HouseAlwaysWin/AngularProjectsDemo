using System;
using EcommerceApi.Core.Entities.Identity;

namespace EcommerceApi.Core.Models.Entities
{
    public class Message: BaseEntity 
    {
        public int SenderId { get; set; }
        public string SenderUsername { get; set; }
        public ECUser Sender { get; set; }
        public int RecipientId { get; set; }
        public string RecipientUsername { get; set; }
        public ECUser Recipient { get; set; }
        public string Content { get; set; }
        public DateTime? DateRead { get; set; }
        public DateTime MessageSent { get; set; } = DateTime.UtcNow;
        public bool SenderDeleted { get; set; }
        public bool RecipientDeleted { get; set; }
    }
}