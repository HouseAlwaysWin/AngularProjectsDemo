using BackendApi.Core.Models.Entities;
using BackendApi.Core.Models.Entities.Identity;

namespace BackendApi.Core.Models.Dtos
{
    public class NotificationListDto
    {
        public PagedList<NotificationDto>  Notifications { get; set; }
        public int NotReadTotalCount { get; set; }
    }
}