using System.Linq;
using System.Threading.Tasks;
using BackendApi.Core.Data.Repositories.Interfaces;
using BackendApi.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BackendApi.Core.Services
{
    public class MessageService
    {
        
        private readonly IIdentityRepository<Message>  _messageRepo;

        public MessageService(IIdentityRepository<Message> messageRepo)
        {
            this._messageRepo = messageRepo; 
        }


        public async Task AddMessage(Message message){
            await _messageRepo.AddAsync(message);
        }

        public async Task DeleteMessage(Message message){
            await _messageRepo.RemoveAsync(message);
        }

        public async Task<Message> GetMessage(int id){
            return await _messageRepo.GetByAsync(query =>{
                return query.Where(m => m.Id ==id)
                            .Include(u => u.Sender)
                            .Include(u => u.Recipient);
            });
        }


    }
}