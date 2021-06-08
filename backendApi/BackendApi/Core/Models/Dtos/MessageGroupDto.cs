using System.Collections.Generic;
using BackendApi.Core.Models.Entities;
using BackendApi.Core.Models.Entities.Identity;

namespace BackendApi.Core.Models.Dtos
{
    public class MessageGroupDto
    {
        
       public int Id { get; set; }
       public string Name { get; set; }
       
       public List<MessageDto> Messages {get; set;} = new List<MessageDto>();

    }
}