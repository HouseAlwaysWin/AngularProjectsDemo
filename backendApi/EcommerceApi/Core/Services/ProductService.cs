using System.Collections.Generic;
using System.Threading.Tasks;
using EcommerceApi.Core.Data.Repositories.Interfaces;
using EcommerceApi.Core.Models.Entities;
using EcommerceApi.Core.Services.Interfaces;

namespace EcommerceApi.Core.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepo;
        public ProductService(IProductRepository productRepo)
        {
           this._productRepo = productRepo;
        }

        public async Task<List<ProductCategory>> GetProductCategoriesTree()
        {
            var parents = await  _productRepo.GetProductCategoriesRootAsync();
            List<ProductCategory> tree = new List<ProductCategory>();
            foreach(var parent in parents){
                var category = await ProductCategoryTreeAsync(parent);
                tree.Add(category);
            }
            return tree;
        }

         private async Task<ProductCategory> ProductCategoryTreeAsync(ProductCategory category){
            if(category.HasChild){
                var children = await  _productRepo.GetProductCategoriesByIdAsync(category.Id);
                category.Children = children;
                foreach(var child in children){
                    await ProductCategoryTreeAsync(child);
                }
            }
            return category;
        }
    }
}