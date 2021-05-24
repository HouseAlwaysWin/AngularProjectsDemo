// using BackendApi.Core.Data.Identity;
// using BackendApi.Core.Data.Repositories.Interfaces;
// using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore;

// namespace BackendApi.Core.Data.Repositories
// {
//     public class UserUow : UowBase<UserContext>,IUserUow
//     {
//        private readonly UserContext _context;
//        public UserUow(UserContext context):base(context)
//        {
//            _context = context;
//        } 
//     }
// }