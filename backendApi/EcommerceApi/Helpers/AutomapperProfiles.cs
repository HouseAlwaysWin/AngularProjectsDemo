

using System.Collections.Generic;
using AutoMapper;
using EcommerceApi.Core.Entities.Identity;
using EcommerceApi.Core.Models.Dtos;
using EcommerceApi.Core.Models.Entities;

namespace EcommerceApi.Helpers.Localization {
    public class AutomapperProfiles:Profile{
        public AutomapperProfiles()
        {
           CreateMap<Product,GetProductDto>()
            .ForMember(pd => pd.ProductCategoryId,m => m.MapFrom(p => p.ProductCategoryId))
            .ForMember(pd => pd.ProductBrandId,m => m.MapFrom(p => p.ProductBrandId))
            .ForMember(pd => pd.ImgUrl,m => m.MapFrom<ProductUrlResolver>());
        
            CreateMap<AddressDto,UserAddress>().ReverseMap();
            CreateMap<AddressDto,OrderAddress>().ReverseMap();

            CreateMap<BasketItem,OrderItem>()
                .IgnoreAllPropertiesWithAnInaccessibleSetter()
                .ReverseMap();
            
            CreateMap<OrderItem,OrderItemDto>();

            CreateMap<Order,OrderDto>()
                .ForMember(o => o.OrderStatus,od => od.MapFrom(od => od.OrderStatus.ToString()))
                .ForMember(o => o.CreatedDate,od => od.MapFrom(od => od.CreatedDate.ToString()));
            


        }
    }
}