using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceApi.Core.Data.Repositories.Interfaces;
using EcommerceApi.Core.Models.Dtos;
using EcommerceApi.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApi.Core.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly StoreContext _context;
        public ProductRepository(StoreContext context)
        {
           this._context = context; 
        }
        // public async Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync()
        // {
        //     return await _context.ProductBrands.ToListAsync();
        // }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products
                    .Include(p=>p.ProductAttributeMap)
                    .Include(p => p.ProductPictureMap)
                    .Include(p=>p.ProductCategory)
                    .FirstOrDefaultAsync(p=>p.Id == id);
        }

        public async Task<List<ProductCategory>> GetProductCategoriesAsync()
        {
            return await _context.ProductCategories.ToListAsync();
        }

        public async Task<List<ProductCategory>> GetProductCategoriesRootAsync(){
            return await _context.ProductCategories.Where(p=>!p.ParentId.HasValue).ToListAsync();
        }

        public async Task<List<ProductCategory>> GetProductCategoriesByIdAsync(int id){
           return await  _context.ProductCategories
                        .Where(p => p.ParentId == id).OrderBy(p => p.SeqNo).ToListAsync();
        }

        public async Task<List<Product>> GetProductsAsync(ProductListParam param)
        {
            IQueryable<Product> query =  _context.Products.Where(p => 
                (string.IsNullOrEmpty(param.Search) || p.Name.ToLower().Contains(param.Search)) &&
                (!param.CategoryId.HasValue || p.ProductCategoryId == param.CategoryId));

            if(param.LoadPictures){
              query =  query.Include(p => p.ProductPictureMap
              ).ThenInclude(p => p.Picture);
            }

            if(param.LoadAttributes){
              query= query.Include(p => p.ProductAttributeMap)
                .ThenInclude(p => p.ProductAttribute)
                .ThenInclude(pa => pa.ProductAttributeValue);
            }

            if(param.LoadCategories){
                query= query.Include(p => p.ProductCategory);
            }

             return await query.ToListAsync();

        }
    }
}