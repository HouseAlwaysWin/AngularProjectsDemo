using AutoMapper;
using BackendApi.Core.Models.Dtos;
using BackendApi.Core.Models.Entities;
using Microsoft.Extensions.Localization;

namespace BackendApi.Helpers.MapResolvers
{
    public class OrderStatusResolver : IValueResolver<Order, OrderDto, string>
    {
        private readonly IStringLocalizer _localizer;
        public OrderStatusResolver(IStringLocalizer localizer)
        {
           this._localizer =localizer; 
        }
        public string Resolve(Order source, OrderDto destination, string destMember, ResolutionContext context)
        {
           string status = _localizer[source.OrderStatus.ToString()].Value;

           return status;
        }
    }
}