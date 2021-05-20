using EcommerceApi.Core.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace EcommerceApi.Core.Data.Repositories
{
    public class IdentityUow : UowBase<IdentityDbContext>,IIdentityUow
    {
        
    }
}