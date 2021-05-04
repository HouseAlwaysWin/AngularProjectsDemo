using System.Globalization;
using System.Linq;


using System.Collections.Generic;
using AutoMapper;
using EcommerceApi.Core.Entities.Identity;
using EcommerceApi.Core.Models.Dtos;
using EcommerceApi.Core.Models.Entities;
using EcommerceApi.Core.Services.Interfaces;
using Microsoft.Extensions.Localization;
using EcommerceApi.Helpers.MapResolvers;

namespace EcommerceApi.Helpers.Localization {
    public class AutomapperProfiles:Profile{
        private readonly ILocalizedService _localizedService;
        private readonly IStringLocalizer _localizer;


        public AutomapperProfiles(ILocalizedService localizedService,
          IStringLocalizer localizer)
        {
            this._localizedService = localizedService;
            this._localizer = localizer;
        }

        public AutomapperProfiles()
        {

           CreateMap<Product,ProductDto>()
                .ForMember(dto => dto.ProductAttributes,p=> p.MapFrom(p => p.ProductAttributeMap.Select(p=>p.ProductAttribute).ToList()))
                .ForMember(dto => dto.ProductPictures,p=> p.MapFrom(p => p.ProductPictureMap.Select(pp=>pp.Picture).ToList()));
           CreateMap(typeof(PagedList<Product>),typeof(PagedList<ProductDto>));
           CreateMap<ProductAttribute,ProductAttributeDto>();
           CreateMap<ProductAttributeValue,ProductAttributeValueDto>();
           CreateMap<Product_ProductAttribute,Product_ProductAttributeDto>();
           CreateMap<ProductCategory,ProductCategoryDto>();
           CreateMap<Picture,PictureDto>();
           CreateMap<Product_Picture,Product_PictureDto>();
           CreateMap<Localized,LocalizedDto>();
           CreateMap<Language,LanguageDto>();

           CreateMap<ProductListParam,ProductLikeParam>().ReverseMap();
        
            CreateMap<AddressDto,UserAddress>().ReverseMap();
            CreateMap<AddressDto,OrderAddress>().ReverseMap();

            CreateMap<BasketItem,OrderItem>()
                .ForMember(b => b.Id,opt => opt.Ignore())
                .IgnoreAllPropertiesWithAnInaccessibleSetter()
                .ReverseMap();
            
            CreateMap<OrderItem,OrderItemDto>();

            CreateMap<Order,OrderDto>()
                .ForMember(od => od.OrderStatus,o => o.MapFrom<OrderStatusResolver>())
                .ForMember(o => o.CreatedDate,od => od.MapFrom(od => od.CreatedDate.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss")))
                .ForMember(o => o.OrderDate,od => od.MapFrom(od => od.OrderDate.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss")));

            CreateMap<DeliveryMethod,DeliveryMethodDto>();
         


        }
    }
}