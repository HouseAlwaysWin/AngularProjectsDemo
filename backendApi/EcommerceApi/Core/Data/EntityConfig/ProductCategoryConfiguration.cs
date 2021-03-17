using EcommerceApi.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcommerceApi.Core.Data.EntityConfig
{
    public class ProductCategoryConfiguration : IEntityTypeConfiguration<ProductCategory>
    {
        public void Configure(EntityTypeBuilder<ProductCategory> builder)
        {
            builder.Property(o => o.CreatedDate)
                .HasDefaultValueSql("GETDATE()")
                .IsRequired();
        }
    }
}