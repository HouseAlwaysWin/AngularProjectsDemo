using System.Collections.Generic;
using BackendApi.Core.Models.Entities.Identity;

namespace BackendApi.Core.Models.Dtos
{
    public class MessageGroupListDto
    {
       public int Id { get; set; }
       public string AlternateId { get; set; } 
       public string GroupName { get; set; }
       public string GroupOtherName { get; set; }
       public string GroupImg { get; set; }
       public string GroupOtherImg { get; set; }
       public string LastMessages { get; set; }
       public int UnreadCount { get; set; }
       public GroupType GroupType { get; set;}
       public List<MessageDto> Messages {get; set;} = new List<MessageDto>();
    }
}