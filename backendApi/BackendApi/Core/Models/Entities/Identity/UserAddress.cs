using System;
using System.ComponentModel.DataAnnotations.Schema;
using BackendApi.Core.Models.Entities;

namespace BackendApi.Core.Entities.Identity
{
    public class UserAddress: IBaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public int Id { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }
    }
}