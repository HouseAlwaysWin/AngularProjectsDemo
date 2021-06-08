using System.Text.RegularExpressions;
using System;
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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace BackendApi.Core.Services.SignalR
{
    public class MessageHub:Hub
    {
        private readonly IHubContext<PresenceHub> _presenceHub;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepo;
        private readonly IUserService _userService;
        private readonly IMessageService _messageService;
        private readonly PresenceTracker _tracker;

        public MessageHub(
            IHubContext<PresenceHub> presenceHub,
            IMapper mapper,
            IUserRepository userRepo,
            IUserService  userService,
            IMessageService messageService,
            PresenceTracker tracker)
        {
            this._presenceHub = presenceHub;
            this._mapper = mapper;
            this._userRepo = userRepo;
            this._userService = userService;
            this._messageService = messageService;
            this._tracker = tracker;
        }

        private async Task<MessageGroup> GenerateNewGroupAsync(string username,string otherUserName){
            var currentUser = await _userService.GetUserByUserNameAsync(username);
            var otherUser = await _userService.GetUserByUserNameAsync(otherUserName);
            var alternateKey = GetGroupAlternateKey(currentUser.UserName, otherUser.UserName);

            var newGroup = new MessageGroup{
                AlternateId = alternateKey,
                GroupName = currentUser.UserName,
                GroupOtherName =otherUser.UserName,
                GroupImg = currentUser.Photos.FirstOrDefault(p => p.IsMain).Url,
                GroupOtherImg = otherUser.Photos.FirstOrDefault(p => p.IsMain).Url,
                GroupType = GroupType.OneOnOne,
                AppUsers = new List<AppUser_MessageGroup>(){
                    new AppUser_MessageGroup{ 
                        AppUserId = currentUser.Id
                    },
                    new AppUser_MessageGroup{ 
                        AppUserId = otherUser.Id
                    }
                }
            };
            await _userRepo.AddAsync<MessageGroup>(newGroup);

            return newGroup;
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
           var groupId = httpContext.Request.Query["groupId"].ToString();
           var userName = Context.User.GetUserName();
           var otherUserName = httpContext.Request.Query["username"].ToString();
           var userId =  Context.User.GetUserId();
           MessageGroup group = null;
           if(!string.IsNullOrEmpty(groupId)){
             group = await _userRepo.GetByAsync<MessageGroup>(query =>
                query.Where(mg => mg.Id == int.Parse(groupId)));
           }
           else 
           {
             var alternateKey = GetGroupAlternateKey(userName, otherUserName);
             group = await _userRepo.GetByAsync<MessageGroup>(query =>
                query.Where(mg => mg.AlternateId == alternateKey));
           }

           if(group == null){
               group =  await GenerateNewGroupAsync(userName,otherUserName); 
           }

            await Groups.AddToGroupAsync(Context.ConnectionId, group.AlternateId);

           if (_userRepo.HasChanges()) {
                await _userRepo.CompleteAsync();
           }

            var currentUsername = Context.User.GetUserName();
            var messages = await _messageService.GetMessageThread(userId,group.Id);

            

            await Clients.Caller.SendAsync("ReceiveMessageThread", messages);
        }


         public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
        }


        public async Task SendMessage(CreateMessageDto createMessageDto)
        {

            var username =  Context.User.GetUserName();

            var groupId = createMessageDto.MessageGroupId;
            var group = new MessageGroup();
            var message = new Message();

            var sender = await _userService.GetUserByUserNameAsync(username);


            if(groupId == null){
                var alternateId = GetGroupAlternateKey(username, createMessageDto.RecipientUsername);
                group = await _userRepo.GetByAsync<MessageGroup>(query => query.Where(u => u.AlternateId == alternateId));
                groupId = group.Id;

                var  recipient = await _userService.GetUserByUserNameAsync(createMessageDto.RecipientUsername);

                if (username == createMessageDto.RecipientUsername.ToLower())
                    throw new HubException("You cannot send messages to yourself");

             
                if (recipient == null) throw new HubException("Not found user");

                message = new Message
                {
                    Sender = sender,
                    SenderUsername = sender.UserName,
                    Content = createMessageDto.Content,
                    MessageGroupId = groupId.Value
                };

                var connections = await _tracker.GetConnectionsForUser(recipient.UserName);
                if (connections != null)
                {
                    await _presenceHub.Clients.Clients(connections).SendAsync("NewMessageReceived",
                        new { username = sender.UserName });
                }
            }
            else{
                group = await _userRepo.GetByAsync<MessageGroup>(query => query.Where(u => u.Id == groupId)); 
                message = new Message{
                    Sender = sender,
                    SenderUsername = sender.UserName,
                    Content = createMessageDto.Content,
                    MessageGroupId = groupId.Value
                };
            }

            await _messageService.AddMessage(message);

            if (await _userRepo.CompleteAsync())
            {
                await Clients.Group(group.AlternateId).SendAsync("NewMessage", _mapper.Map<Message,MessageDto>(message));
            }
        }

        

        // private async Task<MessageGroup> AddToGroup(string groupName)
        // {
        //     var group = await _userRepo.GetByAsync<MessageGroup>(
        //         query => query.Where(mg => mg.GroupName == groupName)
        //                        .Include(mg => mg.Connections));
        //     var connection = new MessageConnection(Context.ConnectionId, Context.User.GetUserName());

        //     if (group == null)
        //     {
        //         group = new MessageGroup(groupName);
        //         await _userRepo.AddAsync<MessageGroup>(group);
        //     }

        //     group.Connections.Add(connection);

        //     var result = await _userRepo.CompleteAsync();

        //     if (result) return group;

        //     throw new HubException("Failed to join group");
        // }

        private async Task<MessageGroup> RemoveFromMessageGroup()
        {
            var group = await _userRepo.GetByAsync<MessageGroup>(query => query.Where(
                mc => mc.Connections.Any(g => g.MessageConnectionId == Context.ConnectionId))
                .Include(mc => mc.Connections));
            
            await _userRepo.RemoveAsync<MessageConnection>(m => m.MessageConnectionId == Context.ConnectionId);
            if (await _userRepo.CompleteAsync()) return group;

            throw new HubException("Failed to remove from group");
        }
        

        private string GetGroupAlternateKey(string caller, string other)
        {
            var stringCompare = string.CompareOrdinal(caller, other) < 0;
            return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
        }

    }
}