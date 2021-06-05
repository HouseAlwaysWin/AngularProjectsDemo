using System.Collections.Generic;

namespace BackendApi.Core.Models.Entities.Identity
{
    public class MessageGroup
    {
        public MessageGroup()
        {
            
        }
        public MessageGroup(string name)
        {
           this.Name = name; 
        }
       public int Id { get; set; }
       public string Name { get; set; } 
       public ICollection<MessageConnection> Connections { get; set; } = new List<MessageConnection>();
    }
}