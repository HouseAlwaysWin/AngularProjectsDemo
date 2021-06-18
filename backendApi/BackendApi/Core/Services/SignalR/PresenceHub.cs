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
using BackendApi.Helpers;
using BackendApi.Helpers.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace BackendApi.Core.Services.SignalR
{

    [Authorize]
    public class PresenceHub : Hub
    {
    //    private readonly PresenceTracker _tracker;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepo;
        private readonly IUserService _userService;
		private readonly ICachedService _cachedService;

        // private  const string onlineUserKey = "onlineUserKey";

		public PresenceHub(
            IMapper mapper,
            IUserRepository userRepo,
            IUserService userService,
            ICachedService cachedService
            )
       {
            this._mapper = mapper;
            this._userRepo = userRepo;
            this._userService = userService;
			this._cachedService = cachedService;
       }

        public async Task SendFriendRequest(int friendId){

            var userId = Context.User.GetUserId();
            var userName = Context.User.GetUserName();
            var requestFriend = await _userRepo.GetByAsync<AppUser>(query => query.Where(u => u.Id == friendId));

            var notification = new Notification {
                AppUserId = friendId,
                ReadDate = null,
                Content="Ask to be friends",
                RequestUserId = userId,
                NotificationType = NotificationType.FriendRequest,
            };

            await _userRepo.AddAsync<Notification>(notification);
            await _userRepo.CompleteAsync();
            
            // var connections = await _tracker.GetConnectionsForUser(requestFriend.UserName);
            var connections = await _cachedService.GetAsync<Dictionary<string,string>>(CachedKeyHelper.UserOnline);

            // if(connections!= null){
            if(connections.ContainsKey(requestFriend.UserName)){
                var notificationsDto = await _userService.GetNotificationDtoAsync(1,10,requestFriend.Id);
                var notReadCount = await _userRepo.GetTotalCountAsync<Notification>(query => query.Where(n => n.ReadDate == null));
                var notificationsListDto = new NotificationListDto{
                    Notifications = notificationsDto,
                    NotReadTotalCount = notReadCount
                };
                await Clients.Client(connections[requestFriend.UserName]).SendAsync("GetNotifications", notificationsListDto);
            }
        }

        public override async Task OnConnectedAsync()
        {
            // var isOnline = await _tracker.UserConnected(Context.User.GetUserName(), Context.ConnectionId);
            var username = Context.User.GetUserName();
            var onlineUsers = await _cachedService.GetAndSetAsync<Dictionary<string,string>>(CachedKeyHelper.UserOnline, 
                new Dictionary<string, string> {
                    {username,Context.ConnectionId} 
                });

            if (!onlineUsers.ContainsKey(username)){
                onlineUsers.Add(username,Context.ConnectionId);
                await _cachedService.SetAsync<Dictionary<string,string>>(CachedKeyHelper.UserOnline,onlineUsers);
            }

            await Clients.Others.SendAsync("UserIsOnline", Context.User.GetUserName());
            await Clients.Caller.SendAsync("GetOnlineUsers", onlineUsers.Keys);

            var userId = Context.User.GetUserId();
            var notifications = await _userRepo.GetAllAsync<Notification>(query => 
                query.Where(n => n.AppUserId == userId)
                     .Include(n => n.RequestUser)
                     .ThenInclude(n => n.Photos)
                     .AsSplitQuery());
            var notificationsDto = _mapper.Map<List<Notification>,List<NotificationDto>>(notifications.ToList());
            await Clients.Caller.SendAsync("GetNotifications",notificationsDto);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var username = Context.User.GetUserName();
            // var connections = await _tracker.GetConnectionsForUser(username);
            // var isOffline = await _tracker.UserDisconnected(Context.User.GetUserName(), connections);
            var usersOnline = await _cachedService.GetAsync<Dictionary<string,string>>(CachedKeyHelper.UserOnline);
            if(usersOnline != null && usersOnline.ContainsKey(username)){
                usersOnline.Remove(username);
                await _cachedService.SetAsync<Dictionary<string,string>>(CachedKeyHelper.UserOnline,usersOnline);
                await Clients.Others.SendAsync("UserIsOffline", username);
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}