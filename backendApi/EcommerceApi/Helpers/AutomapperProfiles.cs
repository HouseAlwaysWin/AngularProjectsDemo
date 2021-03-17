

using AutoMapper;
using EcommerceApi.Core.Models.Dtos;
using EcommerceApi.Core.Models.Entities;

namespace EcommerceApi.Helpers.Localization {
    public class AutomapperProfiles:Profile{
        public AutomapperProfiles()
        {
           CreateMap<Product,GetProductDto>()
            .ForMember(pd => pd.ProductCategory,m => m.MapFrom(p => p.Name))
            .ForMember(pd => pd.ProductBrand,m => m.MapFrom(p => p.Name))
            .ForMember(pd => pd.ImgUrl,m => m.MapFrom<ProductUrlResolver>());
        }
    }
}