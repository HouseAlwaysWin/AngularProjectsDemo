using EcommerceApi.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcommerceApi.Core.Data.EntityConfig
{
    public class ProductBrandConguration : IEntityTypeConfiguration<ProductBrand>
    {
        public void Configure(EntityTypeBuilder<ProductBrand> builder)
        {
            builder.Property(o => o.CreatedDate)
                .HasDefaultValueSql("GETDATE()")
                .IsRequired();
            
        }
    }
        
}