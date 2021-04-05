using EcommerceApi.Core.Models.Entities;

namespace EcommerceApi.Core.Data.QuerySpecs
{
    public class ProductIncludeAllSpec:BaseQuerySpec<Product>
    {
         public ProductIncludeAllSpec(int id)
            :base(p => p.Id == id ) {
                AddInclude(p => p.ProductBrand);
                AddInclude(p => p.ProductCategory);
        } 
    }
}