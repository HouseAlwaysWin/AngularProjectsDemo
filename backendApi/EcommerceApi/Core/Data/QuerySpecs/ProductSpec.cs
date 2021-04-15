using System.Linq;
using EcommerceApi.Core.Models.Dtos;
using EcommerceApi.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApi.Core.Data.QuerySpecs
{
    public static class ProductSpec
    {
       public static IQueryable<Product> GetProducts(IQueryable<Product> query,ProductListParam param){
            query =  query.Where(p => 
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
            return query;
       }

        public static IQueryable<Product> GetProductAll(IQueryable<Product> query){
                query =  query.Include(p => p.ProductPictureMap)
                .ThenInclude(p => p.Picture)
                .Include(p => p.ProductAttributeMap)
                .ThenInclude(p => p.ProductAttribute)
                .ThenInclude(pa => pa.ProductAttributeValue)
                .Include(p => p.ProductCategory);
            return query;
       }


       public static IQueryable<Product> GetProductLikeQuery(IQueryable<Product> query,ProductLikeParam param){
           query = query.Where(q => 
                        (string.IsNullOrEmpty(param.Search) || 
                        EF.Functions.Like(q.Name,$"{param.Search}%")));
            
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
            query = query.OrderBy(o => o.Name);
            return query;
       }
    }
}