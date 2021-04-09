using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceApi.Core.Data.Repositories.Interfaces;
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
        public async Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync()
        {
            return await _context.ProductBrands.ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products
                    .Include(p=>p.ProductBrand)
                    .Include(p=>p.ProductCategory)
                    .FirstOrDefaultAsync(p=>p.Id == id);
        }

        public async Task<IReadOnlyList<ProductCategory>> GetProductCategoriesAsync()
        {
            return await _context.ProductCategories.ToListAsync();
        }

        public async Task<List<ProductCategory>> GetProductCategoriesRootAsync(){
            return await _context.ProductCategories.Where(p=>!p.ParentId.HasValue).ToListAsync();
        }

        public async Task<List<ProductCategory>> GetProductCategoriesByIdAsync(int id){
           return await  _context.ProductCategories
                        .Where(p => p.ParentId == id).OrderBy(p => p.Index).ToListAsync();
        }

        public Task<IReadOnlyList<Product>> GetProductsAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}