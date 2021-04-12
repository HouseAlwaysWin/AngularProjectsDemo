using EcommerceApi.Core.Models.Dtos;
using EcommerceApi.Core.Models.Entities;

namespace EcommerceApi.Core.Data.QuerySpecs
{
    public class ProductCountQuerySpec:BaseQuerySpec<Product>
    {
       public ProductCountQuerySpec(ProductListParam param)
            :base(p => 
                (string.IsNullOrEmpty(param.Search) || p.Name.ToLower().Contains(param.Search)) &&
                // (!param.BrandId.HasValue || p.ProductBrandId == param.BrandId) &&
                (!param.CategoryId.HasValue || p.ProductCategoryId == param.CategoryId)) {
        }
    }
}