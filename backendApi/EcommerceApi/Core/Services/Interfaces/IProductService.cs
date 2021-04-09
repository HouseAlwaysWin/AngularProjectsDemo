using System.Collections.Generic;
using System.Threading.Tasks;
using EcommerceApi.Core.Models.Entities;

namespace EcommerceApi.Core.Services.Interfaces
{
    public interface IProductService
    {
         Task<List<ProductCategory>> GetProductCategoriesTree();
    }
}