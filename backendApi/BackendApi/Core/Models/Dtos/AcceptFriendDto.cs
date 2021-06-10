using System.Collections.Generic;
using BackendApi.Core.Models.Entities;

namespace BackendApi.Core.Models.Dtos
{
    public class AcceptFriendDto
    {
        public NotificationListDto Notifications { get; set; }
        public List<UserFriendMapDto> Friends { get; set; }
    }
}