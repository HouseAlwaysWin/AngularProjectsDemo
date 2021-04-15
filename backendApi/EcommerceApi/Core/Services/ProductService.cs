using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EcommerceApi.Core.Data.QuerySpecs;
using EcommerceApi.Core.Data.Repositories.Interfaces;
using EcommerceApi.Core.Models.Dtos;
using EcommerceApi.Core.Models.Entities;
using EcommerceApi.Core.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApi.Core.Services
{
    public class ProductService : IProductService
    {
        private readonly IEntityRepository<Product> _productEntityRepo;
        private readonly IEntityRepository<ProductCategory> _categoryEntityRepo;
        private readonly IRedisCachedService _redisCachedService;
        private readonly ILocalizedService _localizedService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private string productCahcedKey;
        private string categoryCahcedKey;

        public ProductService(
            IEntityRepository<Product> productEntityRepo,
            IEntityRepository<ProductCategory> categoryEntityRepo,
            IRedisCachedService redisCachedService,
            ILocalizedService localizedService,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            this._localizedService = localizedService;
            this._httpContextAccessor = httpContextAccessor;
            this._mapper = mapper;
            this._productEntityRepo = productEntityRepo;
            this._categoryEntityRepo = categoryEntityRepo;
            this._redisCachedService = redisCachedService;
        }

        private async Task<string> SetAllProductsToCachedReturnKeyAsync()
        {
            var currentLang = _httpContextAccessor.HttpContext.Request.Headers["Accept-Language"].FirstOrDefault();
            var key = $"{currentLang}_all_products_key";
            var products = await _productEntityRepo.GetAllAsync(null, true, key);
            return key;
        }

        private async Task<string> SetAllProductCategoriesToCachedReturnKeyAsync()
        {
            var currentLang = _httpContextAccessor.HttpContext.Request.Headers["Accept-Language"].FirstOrDefault();
            var key = $"{currentLang}_all_productCagetories_key";
            var products = await _categoryEntityRepo.GetAllAsync(null, true, key);
            return key;
        }

        public async Task<PagedList<Product>> GetProductsPagedAsync(ProductListParam param, bool useCached = false)
        {
            PagedList<Product> products = null;
            if (useCached)
            {
                if (!string.IsNullOrEmpty(productCahcedKey))
                {
                    productCahcedKey = await this.SetAllProductsToCachedReturnKeyAsync();
                }
                var data = await this._redisCachedService.GetAsync<List<Product>>(productCahcedKey);
                var totalCount = data.Count();
                var pagedData = data.Skip(param.PageIndex * param.PageSize).Take(param.PageSize).ToList();
                products = new PagedList<Product>(pagedData, totalCount);
            }
            else
            {
                products = await _productEntityRepo.GetAllPagedAsync(
                param.PageIndex, param.PageSize, query => ProductSpec.GetProducts(query, param));
            }

            foreach (var item in products.Data)
            {
                item.Name = await _localizedService.GetLocalizedAsync(item, p => p.Name);
                item.Description = await _localizedService.GetLocalizedAsync(item, p => p.Description);
            }
            return products;
        }

        public async Task<List<Product>> GetProductsLikeAsync(ProductLikeParam param, bool useCached = false)
        {
            var products = await _productEntityRepo.GetAllAsync(query => ProductSpec.GetProductLikeQuery(query, param));

            foreach (var item in products)
            {
                item.Name = await _localizedService.GetLocalizedAsync(item, p => p.Name);
                item.Description = await _localizedService.GetLocalizedAsync(item, p => p.Description);
            }
            return products.ToList();
        }

        public async Task<PagedList<Product>> GetProductsLikePagedAsync(ProductLikeParam param, bool useCached = false)
        {
            var products = await _productEntityRepo.GetAllPagedAsync(
                param.PageIndex, param.PageSize, query => ProductSpec.GetProductLikeQuery(query, param));

            foreach (var item in products.Data)
            {
                item.Name = await _localizedService.GetLocalizedAsync(item, p => p.Name);
                item.Description = await _localizedService.GetLocalizedAsync(item, p => p.Description);
            }
            return products;
        }

        public async Task<Product> GetProductByIdAsync(int id, bool useCached = false)
        {
            var product = await _productEntityRepo.GetByIdAsync(id);
            return product;
        }


        public async Task<List<ProductCategory>> GetProductCategoriesTree(bool useCached = false)
        {
            List<ProductCategory> categories = new List<ProductCategory>();
            if (useCached)
            {
                if (!string.IsNullOrEmpty(categoryCahcedKey))
                {
                    categoryCahcedKey = await this.SetAllProductCategoriesToCachedReturnKeyAsync();
                }
                categories = await this._redisCachedService.GetAsync<List<ProductCategory>>(categoryCahcedKey);
            }
            else
            {
                var getCategories = await _categoryEntityRepo.GetAllAsync(query => query.Where(p => !p.ParentId.HasValue));
                categories = getCategories.ToList();
            }

            List<ProductCategory> tree = new List<ProductCategory>();
            foreach (var parent in categories)
            {
                parent.Name = await _localizedService.GetLocalizedAsync(parent, p => p.Name);
                var category = await ProductCategoryTreeAsync(parent);
                tree.Add(category);
            }
            return tree;
        }

        private async Task<ProductCategory> ProductCategoryTreeAsync(ProductCategory category)
        {
            if (category.HasChild)
            {
                // var children = await  _productRepo.GetProductCategoriesByIdAsync(category.Id);
                var children = await _categoryEntityRepo.GetAllAsync(
                        q => q.Where(p => p.ParentId == category.Id).OrderBy(p => p.SeqNo));

                category.Children = children.ToList();
                category.Name = await _localizedService.GetLocalizedAsync(category, p => p.Name);
                foreach (var child in children)
                {
                    child.Name = await _localizedService.GetLocalizedAsync(child, p => p.Name);
                    await ProductCategoryTreeAsync(child);
                }
            }
            return category;
        }
    }
}