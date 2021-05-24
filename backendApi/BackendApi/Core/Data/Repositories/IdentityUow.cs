using BackendApi.Core.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BackendApi.Core.Data.Repositories
{
    public class IdentityUow : UowBase<IdentityDbContext>,IIdentityUow
    {
        
    }
}