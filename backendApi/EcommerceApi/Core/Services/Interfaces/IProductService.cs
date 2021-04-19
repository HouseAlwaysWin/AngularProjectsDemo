using System.Collections.Generic;
using System.Threading.Tasks;
using EcommerceApi.Core.Models.Dtos;
using EcommerceApi.Core.Models.Entities;
using EcommerceApi.Helpers;

namespace EcommerceApi.Core.Services.Interfaces
{
    public interface IProductService
    {
       Task<Product> GetProductByIdAsync(int id, bool useCached = false);
        Task<List<ProductCategoryDto>> GetProductCategoriesTree(bool useCached = false);
        Task<List<ProductDto>> GetProductsLikeAsync(ProductLikeParam param, bool useCached = false);
        Task<PagedList<ProductDto>> GetProductsLikePagedAsync(ProductLikeParam param, bool useCached = false);
        Task<PagedList<ProductDto>> GetProductsPagedAsync(ProductListParam param, bool useCached = false);
    }
}