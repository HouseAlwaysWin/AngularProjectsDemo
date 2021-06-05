using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BackendApi.Core.Data.Repositories.Interfaces;
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

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var otherUser = httpContext.Request.Query["username"].ToString();
            var username = Context.User.GetUserName();
            if(string.IsNullOrEmpty(otherUser) || string.IsNullOrEmpty(username)){
                throw new ArgumentNullException("username or otherUser is null");
            }

            var groupName = GetGroupName(username, otherUser);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            var group = await AddToGroup(groupName);
            await Clients.Group(groupName).SendAsync("UpdatedGroup", group);

            var currentUsername = Context.User.GetUserName();
            var messages = await _messageService.GetMessageThread(currentUsername,otherUser);

            if (_userRepo.HasChanges()) {
                await _userRepo.CompleteAsync();
            }

            await Clients.Caller.SendAsync("ReceiveMessageThread", messages);
        }


         public override async Task OnDisconnectedAsync(Exception exception)
        {
            var group = await RemoveFromMessageGroup();
            await Clients.Group(group.Name).SendAsync("UpdatedGroup", group);
            await base.OnDisconnectedAsync(exception);
        }


        public async Task SendMessage(CreateMessageDto createMessageDto)
        {
            var username = Context.User.GetUserName();

            if (username == createMessageDto.RecipientUsername.ToLower())
                throw new HubException("You cannot send messages to yourself");

            var sender = await _userService.GetUserByUserNameAsync(username);
            var recipient = await _userService.GetUserByUserNameAsync(createMessageDto.RecipientUsername);

            if (recipient == null) throw new HubException("Not found user");

            var message = new Message
            {
                Sender = sender,
                Recipient = recipient,
                SenderUsername = sender.UserName,
                RecipientUsername = recipient.UserName,
                Content = createMessageDto.Content
            };

            var groupName = GetGroupName(sender.UserName, recipient.UserName);

            var group = await _messageService.GetMessageGroup(groupName);

            if (group.Connections.Any(x => x.UserName == recipient.UserName))
            {
                message.DateRead = DateTime.UtcNow;
            }
            else
            {
                var connections = await _tracker.GetConnectionsForUser(recipient.UserName);
                if (connections != null)
                {
                    await _presenceHub.Clients.Clients(connections).SendAsync("NewMessageReceived",
                        new { username = sender.UserName });
                }
            }

            await _messageService.AddMessage(message);

            if (await _userRepo.CompleteAsync())
            {
                await Clients.Group(groupName).SendAsync("NewMessage", _mapper.Map<Message,MessageDto>(message));
            }
        }


        private async Task<MessageGroup> AddToGroup(string groupName)
        {
            var group = await _userRepo.GetByAsync<MessageGroup>(
                query => query.Where(mg => mg.Name == groupName)
                               .Include(mg => mg.Connections));
            var connection = new MessageConnection(Context.ConnectionId, Context.User.GetUserName());

            if (group == null)
            {
                group = new MessageGroup(groupName);
                await _userRepo.AddAsync<MessageGroup>(group);
            }

            group.Connections.Add(connection);

            var result = await _userRepo.CompleteAsync();

            if (result) return group;

            throw new HubException("Failed to join group");
        }

        private async Task<MessageGroup> RemoveFromMessageGroup()
        {
            var group = await _userRepo.GetByAsync<MessageGroup>(query => query.Where(
                mc => mc.Connections.Any(g => g.MessageConnectionId == Context.ConnectionId))
                .Include(mc => mc.Connections));
            
            await _userRepo.RemoveAsync<MessageConnection>(m => m.MessageConnectionId == Context.ConnectionId);
            if (await _userRepo.CompleteAsync()) return group;

            throw new HubException("Failed to remove from group");
        }
        

        private string GetGroupName(string caller, string other)
        {
            var stringCompare = string.CompareOrdinal(caller, other) < 0;
            return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
        }

    }
}