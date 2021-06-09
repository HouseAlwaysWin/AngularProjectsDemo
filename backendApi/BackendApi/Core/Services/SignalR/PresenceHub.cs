using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BackendApi.Core.Data.Repositories.Interfaces;
using BackendApi.Core.Entities.Identity;
using BackendApi.Core.Models.Dtos;
using BackendApi.Core.Models.Entities.Identity;
using BackendApi.Helpers.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace BackendApi.Core.Services.SignalR
{

    [Authorize]
    public class PresenceHub : Hub
    {
       private readonly PresenceTracker _tracker;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepo;
       public PresenceHub(
            IMapper mapper,
            IUserRepository userRepo,
            PresenceTracker tracker)
       {
            this._mapper = mapper;
            this._userRepo = userRepo;
            _tracker = tracker;
       }

        public async Task SendFriendRequest(int friendId){

            var userId = Context.User.GetUserId();
            var requestFriend = await _userRepo.GetByAsync<AppUser>(query => query.Where(u => u.Id == friendId));

            var notification = new Notification {
                AppUserId = friendId,
                IsRead = false,
                Content="Ask to be friends",
                RequestUserId = userId,
                NotificationType = NotificationType.FriendRequest,
            };

            await _userRepo.AddAsync<Notification>(notification);
            await _userRepo.CompleteAsync();

            var notifications = await _userRepo.GetAllAsync<Notification>(query => 
                query.Where(n=> n.AppUserId == friendId)
                    .Include(n => n.AppUser)
                    .Include(n => n.RequestUser)
                    .AsSplitQuery()
                );
            var notificationsDto = _mapper.Map<List<Notification>,List<NotificationDto>>(notifications.ToList());

            var connections = await _tracker.GetConnectionsForUser(requestFriend.UserName);

            if(connections!= null){
                await Clients.Clients(connections).SendAsync("GetNotifications", notificationsDto);
            }
        }

        public override async Task OnConnectedAsync()
        {
            var isOnline = await _tracker.UserConnected(Context.User.GetUserName(), Context.ConnectionId);
            if (isOnline)
                await Clients.Others.SendAsync("UserIsOnline", Context.User.GetUserName());

            var currentUsers = await _tracker.GetOnlineUsers();
            await Clients.Caller.SendAsync("GetOnlineUsers", currentUsers);

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
            var isOffline = await _tracker.UserDisconnected(Context.User.GetUserName(), Context.ConnectionId);
            
            if (isOffline)
                await Clients.Others.SendAsync("UserIsOffline", Context.User.GetUserName());

            await base.OnDisconnectedAsync(exception);
        }
    }
}