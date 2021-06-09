using System;
using BackendApi.Core.Entities.Identity;

namespace BackendApi.Core.Models.Entities.Identity
{
    public enum NotificationType{
        FriendRequest,
        CommanMessage
    }

    public class Notification
    {
        public int Id { get; set; }
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public int RequestUserId { get; set; }
        public AppUser RequestUser { get; set; }
        public string Content { get; set; }
        public bool IsRead { get; set; }
        public NotificationType NotificationType { get; set; }
        public DateTimeOffset CreatedDate { get; set; }

    }
}