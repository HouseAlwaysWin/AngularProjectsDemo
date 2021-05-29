using System.Linq;
using System.Threading.Tasks;
using BackendApi.Core.Data.Repositories.Interfaces;
using BackendApi.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BackendApi.Core.Services
{
    public class MessageService
    {
        
        private readonly IUserRepository  _userRepo;

        public MessageService(IUserRepository userRepo)
        {
            this._userRepo = userRepo; 
        }

        public async Task AddMessage(Message message){
            await _userRepo.AddAsync(message);
        }

        public async Task DeleteMessage(Message message){
            await _userRepo.RemoveAsync<Message>(m => m.Id == message.Id);
        }

        public async Task<Message> GetMessage(int id){
            return await _userRepo.GetByAsync<Message>(query =>{
                return query.Where(m => m.Id ==id)
                            .Include(u => u.Sender)
                            .Include(u => u.Recipient);
            });
        }


    }
}