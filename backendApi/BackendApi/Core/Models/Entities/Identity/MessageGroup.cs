using System.Collections.Generic;
using BackendApi.Core.Entities.Identity;

namespace BackendApi.Core.Models.Entities.Identity
{

    public enum GroupType {
        OneOnOne,
        OneOnMany
    }
    public class MessageGroup
    {
        public MessageGroup()
        {
            
        }
       public int Id { get; set; }
       public string GroupName { get; set; }
       public string GroupOtherName { get; set; }
       public string GroupImg { get; set; }
       public string GroupOtherImg { get; set; }
       public GroupType GroupType { get; set;}
    //    public ICollection<MessageConnection> Connections { get; set; } = new List<MessageConnection>();
       public ICollection<Message> Messages {get; set;} = new List<Message>();
       public ICollection<AppUser_MessageGroup> AppUsers { get; set; } = new List<AppUser_MessageGroup>();
    }
}