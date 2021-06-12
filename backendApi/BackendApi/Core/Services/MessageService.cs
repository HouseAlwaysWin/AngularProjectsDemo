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



        public async Task AddNewMessageGroup(string groupName,List<AppUser> users){
            var appusers = new List<AppUser_MessageGroup>();
            foreach (var user in users)
            {
                appusers.Add(new AppUser_MessageGroup{
                    AppUserId = user.Id,
                    AppUser = user
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

        public async Task RemoveMessageThread(int groupId){
            await _userRepo.RemoveAsync<MessageGroup>(query => query.Id == groupId);
            await _userRepo.CompleteAsync();
        }

        public async Task EditMessage(Message message){
            await _userRepo.UpdateAsync<Message>(m => m.Id == message.Id,
                new Dictionary<Expression<Func<Message, object>>, object>{
                    { e => e.Content, message.Content }
                }
            );
        }

        public async Task AddMessage(Message message)
        {
            await _userRepo.AddAsync(message);
        }

        public async Task DeleteMessage(Message message)
        {
            await _userRepo.RemoveAsync<Message>(m => m.Id == message.Id);
        }

        public async Task<Message> GetMessage(int id)
        {
            return await _userRepo.GetByAsync<Message>(query =>
            {
                return query.Where(m => m.Id == id)
                            .Include(u => u.Sender)
                            .Include(u => u.RecipientUsers);
            });
        }

        public async Task<IEnumerable<MessageDto>> GetMessageThread(int currentUserId,int groupId){

            await _userRepo.UpdateAsync<MessageRecivedUser>(m => m.DateRead == null && m.AppUserId == currentUserId,
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

  

        // public async Task<MessageGroup> GetMessageGroup(string groupName)
        // {
        //     return await _userRepo.GetByAsync<MessageGroup>(query =>
        //             query.Where(mg => mg.AlternateId == groupName)
        //                  .Include(mg => mg.Connections)
        //     );
        // }

    }
}