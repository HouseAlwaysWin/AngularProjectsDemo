// using System;
// using System.Linq;
// using System.Security.Cryptography.X509Certificates;
// using EcommerceApi.Core.Models.Dtos;
// using EcommerceApi.Core.Models.Entities;

// namespace EcommerceApi.Core.Data.QuerySpecs
// {
//     public class GetProductQuerySpec:BaseQuerySpec<Product>
//     {
//         public GetProductQuerySpec(ProductListParam param)
//             :base(p => 
//                 (string.IsNullOrEmpty(param.Search) || p.Name.ToLower().Contains(param.Search)) &&
//                 // (!param.BrandId.HasValue || p.ProductBrandId == param.BrandId) &&
//                 (!param.CategoryId.HasValue || p.ProductCategoryId == param.CategoryId))
//         {
//            AddInclude(p => p.ProductCategory);
//            AddInclude(p => p.ProductAttributeMap);
//            AddInclude(p => p.ProductPictureMap);
//         //    AddInclude(p => p.ProductBrand);
//            ApplyPaging(param.PageSize * param.PageIndex,
//                    param.PageSize);

//            switch(param.Sort){
//                     case "priceAsc":
//                         AddOrderBy(p => p.Price);
//                         break;
//                     case "priceDesc":
//                         AddOrderByDescending(p => p.Price);
//                         break;
//                     default:
//                         AddOrderBy(n => n.Name);
//                         break;
//                 }
            
//         }

//         public GetProductQuerySpec(int id)
//             :base(product => product.Id== id)
//         {
//            AddInclude(p => p.ProductCategory);
//         //    AddInclude(p => p.ProductBrand);
//         }
//     }
// }