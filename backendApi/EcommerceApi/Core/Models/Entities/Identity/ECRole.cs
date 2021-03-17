using System;
using EcommerceApi.Core.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace EcommerceApi.Core.Entities.Identity
{
    public class ECRole:IdentityRole<int>
    {
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }
    }
}