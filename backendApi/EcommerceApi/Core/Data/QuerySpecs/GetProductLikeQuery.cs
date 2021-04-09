using System.Diagnostics.Tracing;
using EcommerceApi.Core.Models.Dtos;
using EcommerceApi.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApi.Core.Data.QuerySpecs
{
    public class GetProductLikeQuery:BaseQuerySpec<Product>
    {
          public GetProductLikeQuery(GetProductLikeParam param)
            :base(p => 
                (string.IsNullOrEmpty(param.Search) || EF.Functions.Like(p.Name,$"{param.Search}%")))
        {
           AddInclude(p => p.ProductCategory);
           AddInclude(p => p.ProductBrand);
           AddOrderBy(n => n.Name);
           ApplyPaging(param.PageSize * param.PageIndex,
                   param.PageSize);
        }

        public GetProductLikeQuery(string search)
            :base(p => (string.IsNullOrEmpty(search) || EF.Functions.Like(p.Name,$"{search}%")))
        {
            
        }
 
    }
}