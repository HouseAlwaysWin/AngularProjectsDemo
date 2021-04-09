using EcommerceApi.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcommerceApi.Core.EntityConfig
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Id).IsRequired();
            builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
            builder.Property (p => p.Description).IsRequired().HasMaxLength(500);
            builder.Property (p => p.Price).HasColumnType ("decimal(18,2)");
            builder.Property (p => p.ImgUrl).IsRequired ();

            builder.HasOne (b => b.ProductBrand).WithMany ()
                .HasForeignKey (p => p.ProductBrandId);

            builder.HasOne (b => b.ProductCategory).WithMany ()
                .HasForeignKey (p => p.ProductCategoryId);

            builder.Property(o => o.CreatedDate)
                .HasDefaultValueSql("GETDATE()")
                .IsRequired();
        }
    }
}