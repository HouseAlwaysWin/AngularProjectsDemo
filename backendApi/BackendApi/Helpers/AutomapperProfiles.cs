using System.Globalization;
using System.Linq;


using System.Collections.Generic;
using AutoMapper;
using BackendApi.Core.Entities.Identity;
using BackendApi.Core.Models.Dtos;
using BackendApi.Core.Models.Entities;
using BackendApi.Core.Services.Interfaces;
using Microsoft.Extensions.Localization;
using BackendApi.Helpers.MapResolvers;
using BackendApi.Core.Models.Entities.Identity;

namespace BackendApi.Helpers.Localization {
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

           CreateMap<UserInfo,UserInfoDto>();
           CreateMap<UserPhoto,UserPhotoDto>();
           CreateMap<UserAddress,AddressDto>();

           CreateMap<Message,MessageDto>()
                .ForMember(m => m.MainPhoto,m => m.MapFrom(m => m.Sender.Photos.FirstOrDefault(p => p.IsMain).Url));

           CreateMap<MessageGroup,MessageGroupDto>()
               .ForMember(m => m.LastMessages,m => m.MapFrom(m => m.Messages.OrderByDescending(m => m.CreatedDate).FirstOrDefault().Content));

           CreateMap<MessageGroup,MessageFriendsGroupDto>()
               .ForMember(mg => mg.GroupName,mg => mg.MapFrom<MessageGroupNameResolver>())
               .ForMember(mg => mg.GroupImg,mg => mg.MapFrom<MessageGroupImgResolver>())
               .ForMember(m => m.LastMessages,m => m.MapFrom(m => m.Messages.OrderByDescending(m => m.CreatedDate).FirstOrDefault().Content));
               
           CreateMap<MessageRecivedUser,MessageRecivedUserDto>();
           CreateMap<AppUser_MessageGroup,AppUser_MessageGroupDto>();
           CreateMap<Notification,NotificationDto>();

           CreateMap<AppUser,AppUserShortDto>()
                .ForMember(u => u.MainPhoto,u => u.MapFrom(u => u.Photos.FirstOrDefault(u => u.IsMain).Url));

           CreateMap<AppUser,AppUserDto>().ReverseMap();
           CreateMap<AppUser,AppUserTokenDto>();
           CreateMap<UserFriend,UserFriendMapDto>().ReverseMap();
           CreateMap<PagedList<Notification>,PagedList<NotificationDto>>();
         


        }
    }
}