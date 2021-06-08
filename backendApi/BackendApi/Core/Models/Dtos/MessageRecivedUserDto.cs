using System;

namespace BackendApi.Core.Models.Dtos
{
    public class MessageRecivedUserDto
    {
        public int Id { get; set; }
        public int AppUserId { get; set; }
        public string UserName { get; set; }
        public string UserMainPhoto { get; set; }
        public DateTimeOffset? DateRead { get; set; }
    }
}