using System;

namespace EcommerceApi.Core.Models.Entities
{
    public abstract class BaseEntity:BaseEntity<int> { }

    public abstract class BaseEntity<T>:ILocalizedEntity
    {
        public BaseEntity() { }
        public BaseEntity(T id)
        {
           this.Id = id; 
        }
        public T Id { get; set; }
        public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? ModifiedDate { get; set; }
    }
}