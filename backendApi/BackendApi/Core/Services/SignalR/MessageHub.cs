using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BackendApi.Core.Data.Repositories.Interfaces;
using BackendApi.Core.Models.Dtos;
using BackendApi.Core.Models.Entities;
using BackendApi.Core.Models.Entities.Identity;
using BackendApi.Helpers.Extensions;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace BackendApi.Core.Services.SignalR
{
    public class MessageHub:Hub
    {
        private readonly IHubContext<PresenceHub> _presenceHub;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepo;
        private readonly PresenceTracker _tracker;

        public MessageHub(
            IHubContext<PresenceHub> presenceHub,
            IMapper mapper,
            IUserRepository userRepo,
            PresenceTracker tracker)
        {
            this._presenceHub = presenceHub;
            this._mapper = mapper;
            this._userRepo = userRepo;
            this._tracker = tracker;
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var otherUser = httpContext.Request.Query["user"].ToString();
            var groupName = GetGroupName(Context.User.GetUserName(), otherUser);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            var group = await AddToGroup(groupName);
            await Clients.Group(groupName).SendAsync("UpdatedGroup", group);

            var currentUsername = Context.User.GetUserName();
            var messages = await _userRepo.GetAllAsync<Message>(query => 
                query.Where(m => m.Recipient.UserName == currentUsername && 
                        m.RecipientDeleted == false && 
                        m.Sender.UserName == otherUser || 
                        m.Recipient.UserName == otherUser && 
                        m.Sender.UserName == currentUsername && 
                        m.SenderDeleted == false
                ).OrderBy(m => m.MessageSent));

            var messagesDto = _mapper.Map<List<Message>,List<MessageDto>>(messages.ToList());
            var unreadMessages = messagesDto.Where(m => 
                    m.DateRead == null && m.RecipientUsername == currentUsername).ToList();
            if(unreadMessages.Any()){
                foreach (var msg in unreadMessages)
                {
                   msg.DateRead = DateTime.UtcNow; 
                }
            }

            if (_userRepo.HasChanges()) await _userRepo.CompleteAsync();

            await Clients.Caller.SendAsync("ReceiveMessageThread", messages);
        }


         public override async Task OnDisconnectedAsync(Exception exception)
        {
            var group = await RemoveFromMessageGroup();
            await Clients.Group(group.Name).SendAsync("UpdatedGroup", group);
            await base.OnDisconnectedAsync(exception);
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

            if (await _userRepo.CompleteAsync()) return group;

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