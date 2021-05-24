using BackendApi.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackendApi.Core.Models.Entities.EntityConfig
{
    public class Product_ProductAttributeConfig : IEntityTypeConfiguration<Product_ProductAttribute>
    {
        public void Configure(EntityTypeBuilder<Product_ProductAttribute> builder)
        {
            builder.HasKey(pa =>  pa.Id);

            builder.HasOne(pa => pa.Product)
                .WithMany(p => p.ProductAttributeMap)
                .HasForeignKey(pa => pa.ProductId);
            
            builder.HasOne(pa => pa.ProductAttribute)
                .WithMany(p => p.ProductAttributeMap)
                .HasForeignKey(pa => pa.ProductAttributeId);
 
        }
    }
}