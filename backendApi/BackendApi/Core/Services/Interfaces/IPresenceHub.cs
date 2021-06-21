using System;
using System.Threading.Tasks;
using BackendApi.Core.Services.SignalR;
using Microsoft.AspNetCore.SignalR;

namespace BackendApi.Core.Services.Interfaces
{
    public interface IPresenceHub
    {
        Task OnConnectedAsync();
		Task OnDisconnectedAsync(Exception exception);
		Task SendFriendRequestAsync(int friendId);
        Task SendMessagesNotReadTotalCountAsync(string username);
        // Task SendToClientMethodAsync(string username,string methodName,object data);
        IHubCallerClients GetClients();
        IGroupManager GetGroups();


    }
}