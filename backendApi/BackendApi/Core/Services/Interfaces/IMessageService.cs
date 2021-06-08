using System.Collections.Generic;
using System.Threading.Tasks;
using BackendApi.Core.Models.Dtos;
using BackendApi.Core.Models.Entities;
using BackendApi.Core.Models.Entities.Identity;

namespace BackendApi.Core.Services.Interfaces
{
    public interface IMessageService
    {
        Task AddMessage(Message message);
        Task DeleteMessage(Message message);
        Task<Message> GetMessage(int id);
        Task<MessageGroup> GetMessageGroup(string groupName);
        // Task<IEnumerable<MessageDto>> GetMessageThread(string currentUsername, string recipientUsername);
        Task<IEnumerable<MessageDto>> GetMessageThread(int currentUserId,int groupId);
    }
}