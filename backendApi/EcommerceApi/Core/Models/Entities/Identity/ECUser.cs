using System;
using System.Collections.Generic;
using EcommerceApi.Core.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace EcommerceApi.Core.Entities.Identity
{
    public class ECUser : IdentityUser<int>
    {
        public string Gender { get; set; }
        public string DisplayName { get; set; }
        public UserAddress Address { get; set; }
        public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? ModifiedDate { get; set; }
        public ICollection<Message> MessagesSent { get; set; }
        public ICollection<Message> MessagesReceived { get; set; }
        public ICollection<ECRole> ECUserRoles { get; set; }
    }
}