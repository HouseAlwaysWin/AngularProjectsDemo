using System;

namespace EcommerceApi.Core.Models.Entities
{
    public class BaseEntity
    {
        public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? ModifiedDate { get; set; }

    }
}