using System.Collections.Generic;
using System.Threading.Tasks;
using BackendApi.Core.Models.Dtos;
using BackendApi.Core.Models.Entities;
using BackendApi.Core.Models.Entities.Identity;

namespace BackendApi.Core.Services.Interfaces
{
    public interface IMessageService
    {
        Task AddMessageAsync(Message message);
        Task DeleteMessageAsync(Message message);
        Task<Message> GetMessageAsync(int id);
        Task<IEnumerable<MessageDto>> GetMessageThreadAsync(int currentUserId,int groupId);
        Task<List<MessageGroupListDto>> GetMessageGroupListAsync(int userId);
        Task<List<MessageGroupListDto>> GetMessageGroupListAsync(string username);
        Task<MessageGroupDto> GetMessageGroupAsync(int groupId,string username);
    }
}