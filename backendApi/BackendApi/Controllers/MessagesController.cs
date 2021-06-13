using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BackendApi.Core.Data.Repositories.Interfaces;
using BackendApi.Core.Entities.Identity;
using BackendApi.Core.Models.Dtos;
using BackendApi.Core.Models.Entities;
using BackendApi.Core.Models.Entities.Identity;
using BackendApi.Core.Services.Interfaces;
using BackendApi.Helpers.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            IMapper mapper,
            IUserRepository userRepo)
        {
            this._userService = userService;
            this._messageService = messageService;
            this._mapper = mapper;
            this._userRepo  = userRepo;
        }



        [HttpGet("get-user-messages")]
        public async Task<ActionResult> GetUserMessagesList([FromQuery]MessageParams messageParams){
            var message = await _userRepo.GetAllAsync<Message>(
                query => query.Where(m => m.SenderUsername == User.GetUserName()));
            
            return BaseApiOk(message);
        }

        [HttpGet("get-messages-list")]
        public async Task<ActionResult> GetMessageList() {

            var user = await _userRepo.GetByAsync<AppUser>(query => 
                query.Where(u => u.Email == User.GetEmail() )
                     .Include(u => u.MessageGroups)
                     .ThenInclude(u => u.MessageGroup)
                     .ThenInclude(u => u.Messages)
                     .OrderBy(u => u.Id)
                     .AsSplitQuery()
                     );

            var groupsDtoMaps = _mapper.Map<List<AppUser_MessageGroup>,List<AppUser_MessageGroupDto>>(user.MessageGroups.ToList());

            List<MessageGroupDto> groupsDto = new List<MessageGroupDto>();
            foreach (var group in groupsDtoMaps)
            {
               group.MessageGroup.LastMessages = group.MessageGroup.Messages.LastOrDefault().Content;
               groupsDto.Add(group.MessageGroup);
            }

            return BaseApiOk(groupsDto);
        }

        [HttpGet("get-messages-friends-list")]
        public async Task<ActionResult> GetMessageFirendsList() {
            var userId = User.GetUserId();
            var groupsDto = await _messageService.GetMessageGroupList(userId);
            return BaseApiOk(groupsDto);
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMessage(int id){
            var username = User.GetUserName();
            var message = await _userRepo.GetByAsync<Message>(query => query.Where(u => u.Id == id));

            if (message.Sender.UserName != username)
                return Unauthorized();

            if (message.Sender.UserName == username) message.SenderDeleted = true;

            message.RecipientDeleted = true;

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