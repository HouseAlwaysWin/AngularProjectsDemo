using System.Collections.Generic;
using System.Threading.Tasks;
using EcommerceApi.Core.Models.Dtos;
using EcommerceApi.Core.Models.Entities;

namespace EcommerceApi.Core.Services.Interfaces
{
    public interface IProductService
    {
        Task<Product> GetProductByIdAsync(int id, bool useCached = false);
        Task<List<ProductCategory>> GetProductCategoriesTree(bool useCached = false);
        Task<List<Product>> GetProductsLikeAsync(ProductLikeParam param, bool useCached = false);
        Task<PagedList<Product>> GetProductsLikePagedAsync(ProductLikeParam param, bool useCached = false);
        Task<PagedList<Product>> GetProductsPagedAsync(ProductListParam param, bool useCached = false);
    }
}