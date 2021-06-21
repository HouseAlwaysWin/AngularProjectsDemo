using AutoMapper;
using BackendApi.Core.Models.Dtos;
using BackendApi.Core.Models.Entities.Identity;
using BackendApi.Helpers.Extensions;
using Microsoft.AspNetCore.Http;

namespace BackendApi.Helpers.MapResolvers
{
	public class MessageGroupImgResolver : IValueResolver<MessageGroup, MessageGroupListDto, string>
	{

        private readonly IHttpContextAccessor _httpContextAccessor;

		public MessageGroupImgResolver(IHttpContextAccessor httpContextAccessor)
        	{
			this._httpContextAccessor = httpContextAccessor;
		}
		public string Resolve(MessageGroup source, MessageGroupListDto destination, string destMember, ResolutionContext context)
		{
			  var username = _httpContextAccessor.HttpContext.User.GetUserName();
		   if(source.GroupName == username){
			return source.GroupOtherImg;
		   }
		   return source.GroupImg;	
		}
	}
}