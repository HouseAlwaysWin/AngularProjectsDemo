using AutoMapper;
using EcommerceApi.Core.Models.Dtos;
using EcommerceApi.Core.Models.Entities;
using Microsoft.Extensions.Configuration;

namespace EcommerceApi.Helpers
{
    public class ProductUrlResolver : IValueResolver<Product, GetProductDto, string>
    {
        private readonly IConfiguration _config;
        public ProductUrlResolver(IConfiguration config)
        {
           this._config = config; 
        }
        public string Resolve(Product source, GetProductDto destination, string destMember, ResolutionContext context)
        {
            if(!string.IsNullOrEmpty(source.ImgUrl)){
                return _config["ApiUrl"] + source.ImgUrl;
            }

            return null;
        }
    }
}