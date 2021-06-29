using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using BackendApi.Core.Data.Repositories.Interfaces;
using BackendApi.Core.Entities.Identity;
using BackendApi.Core.Models.Dtos;
using BackendApi.Core.Models.Entities;
using BackendApi.Core.Models.Entities.Identity;
using BackendApi.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendApi.Core.Services
{

    public class MessageService : IMessageService
    {

        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;

        public MessageService(IUserRepository userRepo,
            IMapper mapper)
        {
            this._mapper = mapper;
            this._userRepo = userRepo;
        }



        public async Task AddNewMessageGroupAsync(string groupName,List<int> userIds){
            var appusers = new List<AppUser_MessageGroup>();
            foreach (var id in userIds)
            {
                appusers.Add(new AppUser_MessageGroup{
                    AppUserId = id,
                });
            }

            var newGroup = new MessageGroup{
                AlternateId = Guid.NewGuid().ToString(),
                GroupName = groupName,
                GroupType = GroupType.OneOnMany,
                AppUsers = appusers
            };

            await _userRepo.AddAsync<MessageGroup>(newGroup);

            await _userRepo.CompleteAsync();
        }

        public async Task RemoveMessageThreadAsync(int groupId){
            await _userRepo.RemoveAsync<MessageGroup>(query => query.Id == groupId);
            await _userRepo.CompleteAsync();
        }

        public async Task EditMessageAsync(Message message){
            await _userRepo.UpdateAsync<Message>(m => m.Id == message.Id,
                new Dictionary<Expression<Func<Message, object>>, object>{
                    { e => e.Content, message.Content }
                }
            );
        }

        public async Task AddMessageAsync(Message message)
        {
            await _userRepo.AddAsync(message);
        }

        public async Task DeleteMessageAsync(Message message)
        {
            await _userRepo.RemoveAsync<Message>(m => m.Id == message.Id);
        }

        public async Task<Message> GetMessageAsync(int id)
        {
            return await _userRepo.GetByAsync<Message>(query =>
            {
                return query.Where(m => m.Id == id)
                            .Include(u => u.Sender)
                            .Include(u => u.RecipientUsers);
            });
        }

        public async Task<MessageDto> GetMessageDtoAsync(int id){
            var newMessage = await _userRepo.GetByAsync<Message>(query => 
                query.Where(u => u.Id == id).Include(m => m.RecipientUsers));

            var newMessageDto = _mapper.Map<Message,MessageDto>(newMessage);
            return newMessageDto;
        }


        public async Task<PagedList<MessageDto>> GetMessageThreadPagedAsync(int pageIndex,int pageSize, int currentUserId,int groupId){
            var messages = await _userRepo.GetAllPagedAsync<Message>(pageIndex,pageSize,query => 
                query.Where(m => m.MessageGroupId == groupId)
                     .Include(m => m.Sender)
                     .ThenInclude(m => m.Photos)
                     .Include(m => m.RecipientUsers)
                     .OrderBy(m => m.MessageSent)
                     .AsSplitQuery()
            );

            PagedList<MessageDto> messagesDto = _mapper.Map<PagedList<Message>, PagedList<MessageDto>>(messages);
            return messagesDto;
        }

        public async Task<IEnumerable<MessageDto>> GetMessageThreadAsync(int currentUserId,int groupId){

            await _userRepo.UpdateAsync<MessageRecivedUser>(m => 
                            m.DateRead == null && 
                            m.AppUserId == currentUserId &&
                            m.MessageGroupId == groupId,
                    new Dictionary<Expression<Func<MessageRecivedUser, object>>, object>{
                        { mr => mr.DateRead, DateTimeOffset.UtcNow}
                    });

            await _userRepo.CompleteAsync();
            var messages = await _userRepo.GetAllAsync<Message>(query => 
                query.Where(m => m.MessageGroupId == groupId)
                     .Include(m => m.Sender)
                     .ThenInclude(m => m.Photos)
                     .Include(m => m.RecipientUsers)
                     .OrderBy(m => m.MessageSent)
                     .AsSplitQuery()
            );

            List<MessageDto> messagesDto = _mapper.Map<List<Message>, List<MessageDto>>(messages.ToList());
            return messagesDto;
        }

         public async Task<List<MessageGroupListDto>> GetMessageGroupListAsync(int userId) {

            var groups = await _userRepo.GetAllAsync<AppUser_MessageGroup>(query => 
                query.Where(g => g.AppUserId == userId)
                     .Include(u => u.MessageGroup)
                     .ThenInclude(u => u.Messages)
                     .AsSplitQuery()
            );

            List<MessageGroupListDto> groupsDto = new List<MessageGroupListDto>();
            foreach (var group in groups)
            {
                // var unreadCount = await _userRepo.GetTotalCountAsync<MessageRecivedUser>(query => 
                //     query.Where(u => u.MessageGroupId == group.MessageGroupId && u.AppUserId != userId && u.DateRead == null));
                var gm = _mapper.Map<MessageGroup,MessageGroupListDto>(group.MessageGroup);
                gm.UnreadCount = await _userRepo.GetTotalCountAsync<MessageRecivedUser>(query => 
                    query.Where(u => u.MessageGroupId == group.MessageGroupId && u.AppUserId == userId && u.DateRead == null));
                groupsDto.Add(gm);
            }
            return groupsDto;
        }

   
         public async Task<List<MessageGroupListDto>> GetMessageGroupListAsync(string username) {

            var groups = await _userRepo.GetAllAsync<AppUser_MessageGroup>(query => 
                query.Where(g => g.AppUser.UserName == username)
                     .Include(u => u.MessageGroup)
                     .ThenInclude(u => u.Messages)
                     .AsSplitQuery()
            );

            List<MessageGroupListDto> groupsDto = new List<MessageGroupListDto>();
            foreach (var group in groups)
            {
                var gm = _mapper.Map<MessageGroup,MessageGroupListDto>(group.MessageGroup);
                gm.UnreadCount = await _userRepo.GetTotalCountAsync<MessageRecivedUser>(query => 
                    query.Where(u => u.MessageGroupId == group.MessageGroupId && u.UserName != username && u.DateRead == null));
                groupsDto.Add(gm);
            }
            return groupsDto;
        }

        public async Task<MessageGroupDto> GetMessageGroupAsync(int groupId,string username) {

             var group = await _userRepo.GetByAsync<MessageGroup>(query => 
                query.Where(m => m.Id == groupId)
                     .Include(m => m.Messages)
                     .AsSplitQuery()
                     );
            var groupDto = _mapper.Map<MessageGroup,MessageGroupDto>(group); 
            // groupDto.UnreadCount =await _userRepo.GetTotalCountAsync<MessageRecivedUser>(query => 
            //         query.Where(u => u.MessageGroupId == group.Id && u.UserName != username && u.DateRead == null));
            
            return groupDto;
        }
  
    }
}