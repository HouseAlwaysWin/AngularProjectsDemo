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
        private readonly IHubContext<PresenceHub> _presenceHubContext;
        private readonly IMapper _mapper;
		private readonly ICachedService _cachedService;
		private readonly IUserRepository _userRepo;
        private readonly IUserService _userService;
        private readonly IMessageService _messageService;

        public MessageHub(
            IHubContext<PresenceHub> presenceHubContext,
            IMapper mapper,
            ICachedService cachedService,
            IUserRepository userRepo,
            IUserService  userService,
            IMessageService messageService
            )
        {
            this._presenceHubContext = presenceHubContext;
            this._mapper = mapper;
			this._cachedService = cachedService;
			this._userRepo = userRepo;
            this._userService = userService;
            this._messageService = messageService;
        }

        public override async Task OnConnectedAsync()
        {
           var httpContext = Context.GetHttpContext();
           int groupId = int.Parse(httpContext.Request.Query["groupId"].ToString());
           var userName = Context.User.GetUserName();
           var otherUserName = httpContext.Request.Query["username"].ToString();
           var currentPage = httpContext.Request.Query["pageIndex"].ToString();
           var userId =  Context.User.GetUserId();

           MessageGroup group = null;

           if(groupId != 0){
             group = await _userRepo.GetByAsync<MessageGroup>(query =>
                query .Where(mg => mg.Id == groupId)
                    .Include(m => m.AppUsers)
                    .ThenInclude(m => m.AppUser)
                    .AsSplitQuery());
           }
      
            // Set chatroom cathed
            var chatroomKey = $"{CachedKeyHelper.ChatRoomUser}_{group.Id}";
            var chatRoomConnections = await _cachedService.GetAsync<Dictionary<string,string>>(chatroomKey);
            if(chatRoomConnections == null){
                await _cachedService.SetAsync<Dictionary<string,string>>(chatroomKey,
                    new Dictionary<string, string>{
                        { userName,Context.ConnectionId}
                    });
            }
            else {
                if(!chatRoomConnections.ContainsKey(userName)){
                    chatRoomConnections.Add(userName,Context.ConnectionId);
                    await _cachedService.SetAsync<Dictionary<string,string>>(chatroomKey, chatRoomConnections);
                }
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, group.Id.ToString());

           if (_userRepo.HasChanges()) {
                await _userRepo.CompleteAsync();
           }

            var currentUsername = Context.User.GetUserName();

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

            await SendMessageNotificationAsync(userName,groupDto.AppUsers,groupDto);

            await Clients.Groups(group.Id.ToString()).SendAsync("ReceiveMessageThread", 
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
            public int GroupId { get; set; }
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

            var groupId = int.Parse(createMessageDto.MessageGroupId);
            var group = new MessageGroup();
            var message = new Message();

            var sender = await _userService.GetUserByUserNameAsync(username);
            var recipient = await _userService.GetUserByUserNameAsync(createMessageDto.RecipientUsername);

            group = await _userRepo.GetByAsync<MessageGroup>(query => query.Where(u => u.Id == groupId));

            groupId = group.Id;

            if (username == createMessageDto.RecipientUsername.ToLower())
                throw new HubException("You cannot send messages to yourself");

            
            if (recipient == null) throw new HubException("Not found user");


            DateTimeOffset? dateRead = null;
            if(group.GroupType == GroupType.OneOnOne){

                var chatroomKey = $"{CachedKeyHelper.ChatRoomUser}_{group.Id}";
                var connection = await _cachedService.GetAsync<Dictionary<string,string>>(chatroomKey);
                if(connection != null && connection.ContainsKey(recipient.UserName)){
                    {
                        dateRead = DateTimeOffset.UtcNow;
                    }
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
                MessageGroupId = groupId,
                RecipientUsers = recipientUsers
            };

            await _messageService.AddMessageAsync(message);
            await _userRepo.CompleteAsync();

            var groupDto = await _messageService.GetMessageGroupAsync(groupId,username);

            // var connections = await _cachedService.GetAsync<Dictionary<string, string>>(CachedKeyHelper.UserOnline);

            await SendMessageNotificationAsync(username,groupDto.AppUsers,groupDto);
           
          //  var key = $"{CachedKeyHelper.ChatRoomUser}_{groupId}";
          //  var userOnlineConnections = await _cachedService.GetAsync<Dictionary<string,string>>(key);
          //  foreach (var conn in userOnlineConnections)
          //  {
              // if(conn.Key != username) {
                  await Clients.OthersInGroup(group.Id.ToString()).SendAsync("MessageGoDown",true);
              // }
          //  }

            var newMessageDto = _mapper.Map<Message,MessageDto>(message);

            await Clients.Group(group.Id.ToString()).SendAsync("NewMessage",new {
                Message = newMessageDto
            } );
        }

        private async Task SendMessageNotificationAsync(string username,List<MessageGroupToAppUsersDto> currentUsers,MessageGroupDto newGroupDto){
            var connections = await _cachedService.GetAsync<Dictionary<string, string>>(CachedKeyHelper.UserOnline);
            var key = $"{CachedKeyHelper.ChatRoomUser}_{newGroupDto.Id}";
            var userOnlineConnections = await _cachedService.GetAsync<Dictionary<string,string>>(key);

            // notify other user
            foreach (var appuser in currentUsers)
            {
                // if(appuser.AppUser.UserName != username && 
                if(connections.ContainsKey(appuser.AppUser.UserName))
                   {
                    // user is not in chatroom 
                    if(!userOnlineConnections.ContainsKey(appuser.AppUser.UserName)){
                        var totalUnreadCount = await _userRepo.GetTotalCountAsync<MessageRecivedUser>(query =>
                            query.Where(u => u.UserName == appuser.AppUser.UserName && u.DateRead == null));
                    
                        await _presenceHubContext.Clients.Client(connections[appuser.AppUser.UserName]).SendAsync("GetMessagesUnreadTotalCount",totalUnreadCount);
  
                        var newGroupWithUnreadCount = await _userRepo.GetTotalCountAsync<MessageRecivedUser>(query => 
                        query.Where(u => u.MessageGroupId == newGroupDto.Id && u.UserName != username && u.DateRead == null));
                        var cloneGroup = CommonHelpers.CloneObject<MessageGroupDto>(newGroupDto);
                        cloneGroup.UnreadCount = newGroupWithUnreadCount;
                        await _presenceHubContext.Clients.Client(connections[appuser.AppUser.UserName]).SendAsync("UpdateGroup", cloneGroup);
                    }
                    // user is in chatroom
                    else{
                        var totalUnreadCount = await _userRepo.GetTotalCountAsync<MessageRecivedUser>(query =>
                            query.Where(u => u.UserName == appuser.AppUser.UserName && u.DateRead == null));
                    
                        await _presenceHubContext.Clients.Client(connections[appuser.AppUser.UserName]).SendAsync("GetMessagesUnreadTotalCount",totalUnreadCount);
                        await _presenceHubContext.Clients.Client(connections[appuser.AppUser.UserName]).SendAsync("UpdateGroup", newGroupDto);
                    }
                }
            }

        }



        private string GetGroupAlternateKey(string caller, string other)
        {
            var stringCompare = string.CompareOrdinal(caller, other) < 0;
            return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
        }

    }
}