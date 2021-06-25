using System.Collections.Generic;
using BackendApi.Core.Models.Entities.Identity;

namespace BackendApi.Core.Models.Dtos
{
    public class AddGroupParam
    {
        public string GroupName {get;set;}
        public List<int> AppUserIds { get; set; }
    }
}