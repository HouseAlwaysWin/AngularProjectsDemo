using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using EcommerceApi.Core.Data.Repositories.Interfaces;
using EcommerceApi.Core.Models.Dtos;
using EcommerceApi.Core.Models.Entities;
using EcommerceApi.Core.Services.Interfaces;

namespace EcommerceApi.Core.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepo;
        private readonly ILocalizedService _localizedService;
        private readonly IMapper _mapper;

        public ProductService(
            IProductRepository productRepo,
            ILocalizedService localizedService,
            IMapper mapper)
        {
            this._localizedService = localizedService;
            this._mapper = mapper;
            this._productRepo = productRepo;
        }

        public async Task<IReadOnlyList<ProductDto>> GetProductDtosAsync(ProductListParam param){
            var products = await _productRepo.GetProductsAsync(param);

            foreach (var item in products)
            {
               item.Name = await _localizedService.GetLocalizedAsync(item,p => p.Name);
               item.Description = await _localizedService.GetLocalizedAsync(item,p => p.Description);
            }

            var data = _mapper.Map<IReadOnlyList<Product>,IReadOnlyList<ProductDto>>(products);

          return data; 
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