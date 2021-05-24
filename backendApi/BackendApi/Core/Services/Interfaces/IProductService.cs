using System.Collections.Generic;
using System.Threading.Tasks;
using BackendApi.Core.Models.Dtos;
using BackendApi.Core.Models.Entities;
using BackendApi.Helpers;

namespace BackendApi.Core.Services.Interfaces
{
    public interface IProductService
    {

        Task AddProductsAsync(IEnumerable<Product> products);
        Task AddProductAsync(Product product);
        Task<ProductDto> GetProductByIdAsync(int id, bool useCached = false);
        Task<List<ProductCategoryDto>> GetProductCategoriesTree(bool useCached = false);
        Task<List<ProductDto>> GetProductsLikeAsync(ProductLikeParam param, bool useCached = false);
        Task<PagedList<ProductDto>> GetProductsLikePagedAsync(ProductLikeParam param, bool useCached = false);
        Task<PagedList<ProductDto>> GetProductsPagedAsync(ProductListParam param, bool useCached = false);
    }
}