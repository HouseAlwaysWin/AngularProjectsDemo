using EcommerceApi.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcommerceApi.Core.Models.Entities.EntityConfig
{
     public class DeliveryMethodConfig : IEntityTypeConfiguration<DeliveryMethod>
    {
        public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
        {
            builder.Property(d => d.Price)
                .HasColumnType("decimal(18,2)")
                .IsRequired();
        }
    }
}