

using AutoMapper;
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

        }
    }
}