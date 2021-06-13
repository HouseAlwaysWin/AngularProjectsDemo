using System.Globalization;
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
		private readonly ICachedService _cachedService;
		private readonly IUserRepository _userRepo;
        private readonly IUserService _userService;
        private readonly IMessageService _messageService;
        // private readonly PresenceTracker _tracker;

        public MessageHub(
            IHubContext<PresenceHub> presenceHub,
            IMapper mapper,
            ICachedService cachedService,
            IUserRepository userRepo,
            IUserService  userService,
            IMessageService messageService
            // PresenceTracker tracker
            )
        {
            this._presenceHub = presenceHub;
            this._mapper = mapper;
			this._cachedService = cachedService;
			this._userRepo = userRepo;
            this._userService = userService;
            this._messageService = messageService;
            // this._tracker = tracker;
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

            var chatroomKey = $"chatroom_{userName}";
            await _cachedService.SetAsync<string>(chatroomKey,Context.ConnectionId);
            await Groups.AddToGroupAsync(Context.ConnectionId, group.AlternateId);

           if (_userRepo.HasChanges()) {
                await _userRepo.CompleteAsync();
           }

            var currentUsername = Context.User.GetUserName();
            var messages = await _messageService.GetMessageThread(userId,group.Id);
            var groups = await _messageService.GetMessageGroupList(userId);
            await Clients.Groups(group.AlternateId).SendAsync("ReceiveMessageThread", new {
                messages,
                groups
            });
        }


         public override async Task OnDisconnectedAsync(Exception exception)
        {
            var userName =  Context.User.GetUserName();
            var chatroomKey = $"chatroom_{userName}";
            await _cachedService.DeleteAsync(chatroomKey);
            // var connections = await _tracker.GetConnectionsForUser(username);
            //     if (connections != null)
            //     {
            //         await _tracker.UserDisconnected(username,connections);
            //     }
            await base.OnDisconnectedAsync(exception);
        }


        public async Task SendMessage(CreateMessageDto createMessageDto)
        {

            var username =  Context.User.GetUserName();

            var groupId = createMessageDto.MessageGroupId;
            var group = new MessageGroup();
            var message = new Message();

            var sender = await _userService.GetUserByUserNameAsync(username);
            var  recipient = await _userService.GetUserByUserNameAsync(createMessageDto.RecipientUsername);

            if(groupId == null){
      
                var alternateId = GetGroupAlternateKey(username, createMessageDto.RecipientUsername);
                group = await _userRepo.GetByAsync<MessageGroup>(query => query.Where(u => u.AlternateId == alternateId));
            }
            else{
                group = await _userRepo.GetByAsync<MessageGroup>(query => 
                    query.Where(u => u.Id == groupId)
                          .Include(u => u.AppUsers)
                          .ThenInclude(u => u.AppUser)
                    ); 
            }

            groupId = group.Id;

            if (username == createMessageDto.RecipientUsername.ToLower())
                throw new HubException("You cannot send messages to yourself");

            
            if (recipient == null) throw new HubException("Not found user");


            DateTimeOffset? dateRead = null;
            if(group.GroupType == GroupType.OneOnOne){
                // var connections = await _tracker.GetConnectionsForUser(recipient.UserName);
                // if (connections != null)
                // {
                //     dateRead = DateTimeOffset.UtcNow;
                // }
                var chatroomKey = $"chatroom_{recipient.UserName}";
                var connection = await _cachedService.GetAsync<string>(chatroomKey);
                if (!string.IsNullOrEmpty(connection))
                {
                    dateRead = DateTimeOffset.UtcNow;
                }
            }

            var recipientUsers = new List<MessageRecivedUser>();
            recipientUsers.Add(new MessageRecivedUser{
                AppUserId = recipient.Id,
                UserName = recipient.UserName,
                UserMainPhoto = recipient.Photos.FirstOrDefault(r => r.IsMain).Url,
                DateRead = dateRead
            });

            // await _userRepo.BulkAddAsync<MessageRecivedUser>(recipientUsers);

            message = new Message
            {
                Sender = sender,
                SenderUsername = sender.UserName,
                Content = createMessageDto.Content,
                MessageGroupId = groupId.Value,
                RecipientUsers = recipientUsers
            };

            await _messageService.AddMessage(message);
            await _userRepo.CompleteAsync();

            // if(group.GroupType == GroupType.OneOnOne){
            //     var connections = await _tracker.GetConnectionsForUser(recipient.UserName);
            //     if (connections != null)
            //     {
            //         await _userRepo.UpdateAsync<MessageRecivedUser>(m => m.DateRead == null && m.AppUserId == recipient.Id,
            //             new Dictionary<string,object> { 
            //                 { "DateRead",  DateTimeOffset.UtcNow} 
            //             });

            //         // await _presenceHub.Clients.Clients(connections).SendAsync("UpdateIsRead",
            //         //     new { username = sender.UserName });
                      
            //     }
            //     // await _userRepo.CompleteAsync();
            // }
            // if(_userRepo.HasChanges()){
                // await _userRepo.CompleteAsync();
                var newMessage = await _userRepo.GetByAsync<Message>(query => 
                    query.Where(u => u.Id == message.Id).Include(m => m.RecipientUsers));
                await Clients.Group(group.AlternateId).SendAsync("NewMessage", _mapper.Map<Message,MessageDto>(message));
            // }   
        }

        // private async Task<MessageGroup> RemoveFromMessageGroup()
        // {
        //     var group = await _userRepo.GetByAsync<MessageGroup>(query => query.Where(
        //         mc => mc.Connections.Any(g => g.MessageConnectionId == Context.ConnectionId))
        //         .Include(mc => mc.Connections));
            
        //     await _userRepo.RemoveAsync<MessageConnection>(m => m.MessageConnectionId == Context.ConnectionId);
        //     if (await _userRepo.CompleteAsync()) return group;

        //     throw new HubException("Failed to remove from group");
        // }
        

        private string GetGroupAlternateKey(string caller, string other)
        {
            var stringCompare = string.CompareOrdinal(caller, other) < 0;
            return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
        }

    }
}