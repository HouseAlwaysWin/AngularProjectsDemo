using System;

namespace EcommerceApi.Core.Models.Entities
{
    public abstract class BaseEntity 
    {
        public int Id { get; set; }
        public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? ModifiedDate { get; set; }
    }
    
}