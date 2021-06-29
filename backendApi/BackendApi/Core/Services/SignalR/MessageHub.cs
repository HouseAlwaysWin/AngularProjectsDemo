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
using Newtonsoft.Json;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;

namespace BackendApi.Core.Services.SignalR
{
    public class MessageHub:Hub
    {
        // private readonly IPresenceHub _presenceHub;
        private readonly IHubContext<PresenceHub> _presenceHubContext;
        private readonly IMapper _mapper;
		private readonly ICachedService _cachedService;
		private readonly IUserRepository _userRepo;
        private readonly IUserService _userService;
        private readonly IMessageService _messageService;

        public MessageHub(
            // IPresenceHub presenceHub,
            IHubContext<PresenceHub> presenceHubContext,
            IMapper mapper,
            ICachedService cachedService,
            IUserRepository userRepo,
            IUserService  userService,
            IMessageService messageService
            )
        {
            // this._presenceHub = presenceHub;
            this._presenceHubContext = presenceHubContext;
            this._mapper = mapper;
			this._cachedService = cachedService;
			this._userRepo = userRepo;
            this._userService = userService;
            this._messageService = messageService;
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
           var currentPage = httpContext.Request.Query["pageIndex"].ToString();
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

            // var chatroomKey = $"{CachedKeyHelper.ChatRoomUser}_{userName}";

            // Set chatroom cathed
            var chatroomKey = $"{CachedKeyHelper.ChatRoomUser}_{group.AlternateId}";
            await _cachedService.SetAsync<Dictionary<string,string>>(chatroomKey,
                new Dictionary<string, string>{
                    { userName,Context.ConnectionId}
                });

            await Groups.AddToGroupAsync(Context.ConnectionId, group.AlternateId);

           if (_userRepo.HasChanges()) {
                await _userRepo.CompleteAsync();
           }

            var currentUsername = Context.User.GetUserName();
            // var messages = await _messageService.GetMessageThreadAsync(userId,group.Id);
            
            // var userGroups = await _messageService.GetMessageGroupListAsync(userName);
            // var otherUserGroups = await _messageService.GetMessageGroupListAsync(otherUserName);

            await _userRepo.UpdateAsync<MessageRecivedUser>(m => m.DateRead == null && m.AppUserId == userId && m.MessageGroupId == group.Id,
                    new Dictionary<Expression<Func<MessageRecivedUser, object>>, object>{
                        { mr => mr.DateRead, DateTimeOffset.UtcNow}
                    });
            await _userRepo.CompleteAsync();

            //  Count unread and rearrange message list
            var totalCount = await _userRepo.GetTotalCountAsync<Message>(query => 
                query.Where(m => m.MessageGroupId == group.Id));
            var pageIndex = (totalCount/10)+1;
            var messages = await _messageService.GetMessageThreadPagedAsync(pageIndex,10,userId,group.Id);
            if(messages.Data.Count < 10 && pageIndex > 1){
                pageIndex = (totalCount/10)-1;
                var prevMsg = await _messageService.GetMessageThreadPagedAsync(pageIndex,10,userId,group.Id);
                prevMsg.Data.AddRange(messages.Data);
                messages.Data = prevMsg.Data;
            }

            var groupDto = _mapper.Map<MessageGroup,MessageGroupDto>(group); 

           
            var totalUnreadCount =await _userRepo.GetTotalCountAsync<MessageRecivedUser>(query =>
                            query.Where(u => u.UserName == userName && u.DateRead == null));

            // var connections = await _cachedService.GetAsync<Dictionary<string, string>>(CachedKeyHelper.UserOnline);
            await SendMessageNotificationAsync(userName,groupDto.AppUsers,groupDto);

            // if(connections.ContainsKey(userName)){
            //     await _presenceHubContext.Clients.Client(connections[userName]).SendAsync("GetMessagesUnreadTotalCount",totalUnreadCount);
            //     groupDto.UnreadCount = await _userRepo.GetTotalCountAsync<MessageRecivedUser>(query => 
            //         query.Where(u => u.MessageGroupId == groupDto.Id && u.UserName == userName && u.DateRead == null));
            //     await _presenceHubContext.Clients.Client(connections[userName]).SendAsync("UpdateGroup", groupDto);
            // }
            

            await Clients.Groups(group.AlternateId).SendAsync("ReceiveMessageThread", 
                new { 
                    messages,
                    pageIndex
                });
        }


         public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
        }

        public class DisconnectParam {
            public string GroupId { get; set; }
        }

        public async Task OnDisconnectChatRoom(DisconnectParam group){
            var userName =  Context.User.GetUserName();
            
            var key = $"{CachedKeyHelper.ChatRoomUser}_{group.GroupId}";
            var chatroomUsers = await _cachedService.GetAsync<Dictionary<string,string>>(key);
            // remove user from chatroom.
            if(chatroomUsers.ContainsKey(userName)){
                chatroomUsers.Remove(userName);
                await _cachedService.SetAsync<Dictionary<string,string>>(key,chatroomUsers);
            }

            if(chatroomUsers.Count == 0){
                await _cachedService.DeleteAsync(key);
            }

        }


        public async Task SendMessageAsync(CreateMessageDto createMessageDto)
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
                MessageGroupId = group.Id,
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

            await _messageService.AddMessageAsync(message);
            await _userRepo.CompleteAsync();

            var groupDto = await _messageService.GetMessageGroupAsync(groupId.Value,username);

            var connections = await _cachedService.GetAsync<Dictionary<string, string>>(CachedKeyHelper.UserOnline);

            // await _presenceHub.SendMessagesNotReadTotalCountAsync(recipient.UserName);
            // await _presenceHub.SendToClientMethodAsync(sender.UserName,"UpdateGroup",groupDto);

            await SendMessageNotificationAsync(username,groupDto.AppUsers,groupDto);
           
            // if(connections.ContainsKey(username)){
            //     foreach (var appuser in groupDto.AppUsers)
            //     {
            //         if(appuser.AppUser.UserName != username){
            //              var totalUnreadCount =await _userRepo.GetTotalCountAsync<MessageRecivedUser>(query =>
            //                 query.Where(u => u.UserName == appuser.AppUser.UserName && u.DateRead == null));

            //             await _presenceHubContext.Clients.Client(connections[appuser.AppUser.UserName]).SendAsync("GetMessagesUnreadTotalCount",totalUnreadCount);
            //         }
            //         await _presenceHubContext.Clients.Client(connections[appuser.AppUser.UserName]).SendAsync("UpdateGroup", groupDto);
            //     }
            // }
            // foreach (var appuser in groupDto.AppUsers)
            // {
            //     await _presenceHub.SendToClientMethodAsync(appuser.AppUser.UserName,"UpdateGroup",groupDto);
            // }

            var newMessageDto = _mapper.Map<Message,MessageDto>(message);
            // newMessageDto.currentPageIndex = 
            await Clients.Group(group.AlternateId).SendAsync("NewMessage",new {
                Message = newMessageDto,
                // Group = groupDto
            } );
        }

        private async Task SendMessageNotificationAsync(string username,List<MessageGroupToAppUsersDto> currentUsers,MessageGroupDto newGroupDto){
            var connections = await _cachedService.GetAsync<Dictionary<string, string>>(CachedKeyHelper.UserOnline);
            foreach (var appuser in currentUsers)
            {
                if(appuser.AppUser.UserName != username && connections.ContainsKey(appuser.AppUser.UserName) ){
                        var totalUnreadCount = await _userRepo.GetTotalCountAsync<MessageRecivedUser>(query =>
                        query.Where(u => u.UserName == appuser.AppUser.UserName && u.DateRead == null));
                    
                    await _presenceHubContext.Clients.Client(connections[appuser.AppUser.UserName]).SendAsync("GetMessagesUnreadTotalCount",totalUnreadCount);
                    var newGroupWithUnreadCount = await _userRepo.GetTotalCountAsync<MessageRecivedUser>(query => 
                        query.Where(u => u.MessageGroupId == newGroupDto.Id && u.UserName != username && u.DateRead == null));
                    var cloneGroup = CommonHelpers.CloneObject<MessageGroupDto>(newGroupDto);
                    cloneGroup.UnreadCount = newGroupWithUnreadCount;
                    await _presenceHubContext.Clients.Client(connections[appuser.AppUser.UserName]).SendAsync("UpdateGroup", cloneGroup);
                }
            }

            if(connections.ContainsKey(username)){
                var totalUnreadCount = await _userRepo.GetTotalCountAsync<MessageRecivedUser>(query =>
                        query.Where(u => u.UserName == username && u.DateRead == null));
                    
                await _presenceHubContext.Clients.Client(connections[username]).SendAsync("GetMessagesUnreadTotalCount",totalUnreadCount);
                await _presenceHubContext.Clients.Client(connections[username]).SendAsync("UpdateGroup", newGroupDto);
            }
        }



        private string GetGroupAlternateKey(string caller, string other)
        {
            var stringCompare = string.CompareOrdinal(caller, other) < 0;
            return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
        }

    }
}