using System.Security.AccessControl;
namespace BackendApi.Core.Models.Entities.Identity
{
    public class MessageConnection
    {
        public MessageConnection()
        {
        }

        public MessageConnection(string connectionId,string username)
        {
           this.MessageConnectionId = connectionId;
           this.UserName = username;
        }
        public string MessageConnectionId { get; set; }
        public string UserName { get; set; }
    }
}