using System;

namespace BackendApi.Core.Models.Entities
{
    public interface IBaseEntity
    {
        int Id { get; set; }
        DateTimeOffset CreatedDate { get; set; } 
        DateTimeOffset? ModifiedDate { get; set; }
    }
    
}