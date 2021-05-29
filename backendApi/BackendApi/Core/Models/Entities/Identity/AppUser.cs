using System;
using System.Collections.Generic;
using BackendApi.Core.Models.Dtos;
using BackendApi.Core.Models.Entities;
using BackendApi.Core.Models.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace BackendApi.Core.Entities.Identity
{
    public class AppUser : IdentityUser<int>,IBaseEntity
    {
        public string UserPublicId { get; set; }
        public UserAddress Address { get; set; }
        public UserInfo UserInfo { get; set; }
        public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? ModifiedDate { get; set; }
        public List<Message> MessagesSent { get; set; }
        public List<Message> MessagesReceived { get; set; }
        public ICollection<AppUserRole> AppUserRoles { get; set; }
        public ICollection<UserPhoto> Photos { get; set; }
        public ICollection<UserRelationship> Relationships { get; set; }
    }
}