using System;
using System.Collections.Generic;
using BackendApi.Core.Entities.Identity;

namespace BackendApi.Core.Models.Entities.Identity
{
    public class UserRelationship:IBaseEntity
    {
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public int RelationshipId { get; set; }
        public AppUser Relationship { get; set; }
        public int Id { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }
    }
}