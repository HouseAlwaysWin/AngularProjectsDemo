using System;
using Microsoft.AspNetCore.Identity;

namespace EcommerceApi.Core.Entities.Identity
{
    public class ECUser : IdentityUser<int>
    {
        public string DisplayName { get; set; }
        public UserAddress Address { get; set; }
    }
}