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
            builder.Property(o => o.Name)
                .HasMaxLength(100);
            
            builder.Property(o => o.LangCode)
                .HasMaxLength(10);

            builder.Ignore(o => o.Children);

        }
    }
}