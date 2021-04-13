using System.Collections.Generic;
using System.Threading.Tasks;
using EcommerceApi.Core.Models.Dtos;
using EcommerceApi.Core.Models.Entities;

namespace EcommerceApi.Core.Data.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> GetProductByIdAsync (int id);
        Task<List<Product>> GetProductsAsync (ProductListParam param);
        // Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync ();
        Task<List<ProductCategory>> GetProductCategoriesAsync ();
        Task<List<ProductCategory>> GetProductCategoriesRootAsync();
        Task<List<ProductCategory>> GetProductCategoriesByIdAsync(int id);
    }
}