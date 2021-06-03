using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BackendApi.Core.Data.Repositories.Interfaces;
using BackendApi.Core.Models.Dtos;
using BackendApi.Core.Models.Entities;
using BackendApi.Core.Services.Interfaces;
using BackendApi.Helpers.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace BackendApi.Controllers
{
    public class MessagesController:BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IMessageService _messageService;
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepo;

        public MessagesController(
            IMessageService messageService,
            IUserService userService,
            IMapper mapper)
        {
            this._userService = userService;
            this._messageService = messageService;
            this._mapper = mapper;
        }



        [HttpGet("get-user-messages")]
        public async Task<ActionResult> GetUserMessagesList([FromQuery]MessageParams messageParams){
            var message = await _userRepo.GetAllAsync<Message>(
                query => query.Where(m => m.RecipientUsername == User.GetUserName()));
            
            return BaseApiOk(message);
        }



        [HttpGet]
        public async Task<ActionResult> GetMessagesForUser([FromQuery]MessageParams messageParams){

            var message = await _userRepo.GetAllPagedAsync<Message>(
                messageParams.PageIndex,messageParams.PageSize,
                query =>{
                    switch(messageParams.Container){
                        case "Inbox":
                            return query.Where(u => u.RecipientUsername == messageParams.Username && u.RecipientDeleted == false);
                        case "Outbox":
                            return query.Where(u => u.SenderUsername == messageParams.Username && u.SenderDeleted == false);
                        default:
                            return query.Where(u => u.RecipientUsername == messageParams.Username && u.RecipientDeleted == false && u.DateRead == null);
                    }
                });
            return BaseApiOk(message);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMessage(int id){
            var username = User.GetUserName();
            var message = await _userRepo.GetByAsync<Message>(query => query.Where(u => u.Id == id));

             if (message.Sender.UserName != username && message.Recipient.UserName != username)
                return Unauthorized();

            if (message.Sender.UserName == username) message.SenderDeleted = true;

            if (message.Recipient.UserName == username) message.RecipientDeleted = true;

            if (message.SenderDeleted && message.RecipientDeleted)
                await _userRepo.RemoveAsync<Message>(m => m.Id == id);
            
            var result = await _userRepo.CompleteAsync();
            if(result){
                return BaseApiOk();
            }

            return BaseApiBadRequest("Delete message failed");
        }
        
    }
}