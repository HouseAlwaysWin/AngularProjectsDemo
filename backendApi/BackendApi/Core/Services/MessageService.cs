using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BackendApi.Core.Data.Repositories.Interfaces;
using BackendApi.Core.Models.Dtos;
using BackendApi.Core.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendApi.Core.Services
{
    public class MessageService
    {
        
        private readonly IUserRepository  _userRepo;
        private readonly IMapper _mapper;

        public MessageService(IUserRepository userRepo,
            IMapper mapper)
        {
            this._mapper = mapper;
            this._userRepo = userRepo; 
        }

        public async Task AddMessage(Message message){
            await _userRepo.AddAsync(message);
        }

        public async Task DeleteMessage(Message message){
            await _userRepo.RemoveAsync<Message>(m => m.Id == message.Id);
        }

        public async Task<Message> GetMessage(int id){
            return await _userRepo.GetByAsync<Message>(query =>{
                return query.Where(m => m.Id ==id)
                            .Include(u => u.Sender)
                            .Include(u => u.Recipient);
            });
        }

        public async Task<IEnumerable<MessageDto>> GetMessageThread(
            string currentUsername, 
            string recipientUsername){
            
            var messages = await _userRepo.GetAllAsync<Message>(query =>
                query.Where(m => 
                    m.Recipient.UserName == currentUsername &&
                    m.RecipientDeleted == false && 
                    m.Sender.UserName == recipientUsername  ||
                    m.Recipient.UserName == recipientUsername &&
                    m.Sender.UserName == currentUsername &&
                    m.SenderDeleted == false
                ).OrderBy(m => m.MessageSent));

            List<MessageDto> messagesDto = _mapper.Map<List<Message>,List<MessageDto>>(messages.ToList());

            var unreadMessages = messagesDto.Where(m => 
                m.DateRead ==null && m.RecipientUsername == currentUsername).ToList();
            
            if(unreadMessages.Any()){
                foreach (var message in unreadMessages)
                {
                   message.DateRead = DateTime.UtcNow; 
                }
            }

            return messagesDto;
        }


    }
}