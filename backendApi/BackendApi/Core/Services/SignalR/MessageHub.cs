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
using BackendApi.Helpers;

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

            var chatroomKey = $"{CachedKeyHelper.ChatRoomUser}_{userName}";
            await _cachedService.SetAsync<string>(chatroomKey,Context.ConnectionId);

            await Groups.AddToGroupAsync(Context.ConnectionId, group.AlternateId);

           if (_userRepo.HasChanges()) {
                await _userRepo.CompleteAsync();
           }

            var currentUsername = Context.User.GetUserName();
            var messages = await _messageService.GetMessageThread(userId,group.Id);
            var userGroups = await _messageService.GetMessageGroupList(userId);
            var otherUserGroups = await _messageService.GetMessageGroupList(otherUserName);

            var groupDto = _mapper.Map<MessageGroup,MessageGroupDto>(group); 

            await Clients.Groups(group.AlternateId).SendAsync("ReceiveMessageThread", new { messages, group = groupDto });
        }


         public override async Task OnDisconnectedAsync(Exception exception)
        {
            var userName =  Context.User.GetUserName();
            // var chatroomKey = $"chatroom_{userName}";
            await _cachedService.DeleteAsync($"{CachedKeyHelper.ChatRoomUser}_{userName}");
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

                var chatroomKey = $"{CachedKeyHelper.ChatRoomUser}_{recipient.UserName}";
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


            var newMessage = await _userRepo.GetByAsync<Message>(query => 
                query.Where(u => u.Id == message.Id).Include(m => m.RecipientUsers));
            var newMessageDto = _mapper.Map<Message,MessageDto>(message);
            // var groups = await _messageService.GetMessageGroupList(sender.Id);
            var groupDto = await _messageService.GetMessageGroup(groupId.Value);

            // var connections = await _cachedService.GetAsync<Dictionary<string,string>>($"{CachedKeyHelper.ChatRoomUser}_{recipientUsers}");

            // foreach (var user in newMessageDto.RecipientUsers)
            // {
            //     if(connections.ContainsKey(user.UserName)){
            //         var msg = newMessageDto.RecipientUsers.FirstOrDefault(r => r.UserName == user.UserName);
            //         msg.DateRead = DateTimeOffset.UtcNow;
            //     }
            // }


            var notification = new Notification {
                AppUserId = recipient.Id,
                ReadDate = null,
                Content="Test",
                RequestUserId = sender.Id,
                NotificationType = NotificationType.CommanMessage,
            };

            await _userRepo.AddAsync<Notification>(notification);
            await _userRepo.CompleteAsync();
            
            // var connections = await _tracker.GetConnectionsForUser(requestFriend.UserName);
            var connections = await _cachedService.GetAsync<Dictionary<string,string>>(CachedKeyHelper.UserOnline);

            // if(connections!= null){
            if(connections.ContainsKey(recipient.UserName)){
                var notificationsDto = await _userService.GetNotificationDtoAsync(1,10,recipient.Id);
                var notReadCount = await _userRepo.GetTotalCountAsync<Notification>(query => query.Where(n => n.ReadDate == null));
                var notificationsListDto = new NotificationListDto{
                    Notifications = notificationsDto,
                    NotReadTotalCount = notReadCount
                };
                await _presenceHub.Clients.Client(connections[recipient.UserName]).SendAsync("GetNotifications", notificationsListDto);
            }


            await Clients.Group(group.AlternateId).SendAsync("NewMessage",new {
                Message = newMessageDto,
                Group = groupDto
            } );
        }


        private string GetGroupAlternateKey(string caller, string other)
        {
            var stringCompare = string.CompareOrdinal(caller, other) < 0;
            return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
        }

    }
}