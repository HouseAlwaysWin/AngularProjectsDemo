using System;
using System.Security.Cryptography.X509Certificates;
using EcommerceApi.Core.Models.Dtos;
using EcommerceApi.Core.Models.Entities;

namespace EcommerceApi.Core.Data.QuerySpecs
{
    public class GetProductQuerySpec:BaseQuerySpec<Product>
    {
        public GetProductQuerySpec(GetProductParam param)
            :base(p => 
                (string.IsNullOrEmpty(param.Search) || p.Name.ToLower().Contains(param.Search)) &&
                (!param.BrandId.HasValue || p.ProductBrandId == param.BrandId) &&
                (!param.CategoryId.HasValue || p.ProductCategoryId == param.CategoryId))
        {
           AddInclude(p => p.ProductCategory);
           AddInclude(p => p.ProductBrand);
           AddOrderBy(p => p.Name);
           ApplyPaging(param.PageSize * (param.PageIndex-1),
                   param.PageSize);
        }

        public GetProductQuerySpec(int id)
            :base(product => product.Id== id)
        {
           AddInclude(p => p.ProductCategory);
           AddInclude(p => p.ProductBrand);
        }
    }
}