using System.Collections.Generic;
using System.Threading.Tasks;
using EcommerceApi.Core.Models.Dtos;
using EcommerceApi.Core.Models.Entities;

namespace EcommerceApi.Core.Services.Interfaces
{
    public interface IProductService
    {
         Task<List<ProductCategory>> GetProductCategoriesTree();
         Task<IReadOnlyList<ProductDto>> GetProductDtosAsync(ProductListParam param);
    }
}