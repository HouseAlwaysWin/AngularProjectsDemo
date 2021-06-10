using System;
using BackendApi.Core.Models.Entities.Identity;

namespace BackendApi.Core.Models.Dtos
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public int AppUserId { get; set; }
        public AppUserShortDto AppUser { get; set; }
      
        public int RequestUserId { get; set; }
        public AppUserShortDto RequestUser { get; set; }
        public string Content { get; set; }
        public DateTimeOffset? ReadDate { get; set; }
        public NotificationType NotificationType { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }
}