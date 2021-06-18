using System;
using BackendApi.Core.Entities.Identity;

namespace BackendApi.Core.Models.Entities.Identity
{
    public enum NotificationType{
        FriendRequest,
        CommanMessage
    }

    public enum QAStatus{

        NoResponse,
        Accept,
        Reject
    }

    public class Notification
    {
        public int Id { get; set; }
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public int RequestUserId { get; set; }
        public AppUser RequestUser { get; set; }
        public string Content { get; set; }
        public QAStatus QAStatus { get; set; } = QAStatus.NoResponse;
        public DateTimeOffset? ReadDate { get; set; }
        public NotificationType NotificationType { get; set; }
        public DateTimeOffset CreatedDate { get; set; }

    }
}