using System;
using System.Collections.Generic;
using BackendApi.Core.Models.Entities;
using BackendApi.Core.Models.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace BackendApi.Core.Entities.Identity
{
    public class AppRole:IdentityRole<int>
    {
        public ICollection<AppUserRole> AppUserRoles { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }
    }
}