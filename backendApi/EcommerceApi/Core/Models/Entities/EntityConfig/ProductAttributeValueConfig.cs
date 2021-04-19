using System.Threading;
using EcommerceApi.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcommerceApi.Core.Models.Entities.EntityConfig
{
    public class ProductAttributeValueConfig : IEntityTypeConfiguration<ProductAttributeValue>
    {
        public void Configure(EntityTypeBuilder<ProductAttributeValue> builder)
        {
            builder.HasKey(pa => pa.Id);
            builder.Property (p => p.PriceAdjustment).HasColumnType ("decimal(18,2)");

            builder.HasOne(v => v.ProductAttribute)
                .WithMany(a => a.ProductAttributeValue)
                .HasForeignKey(v => v.ProductAttributeId);

        }
    }
}