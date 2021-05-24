using BackendApi.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackendApi.Core.Models.Entities.EntityConfig
{
    public class OrderItemConfig:IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {

            builder.HasKey(o => o.Id);
            builder.Property(o => o.CreatedDate)
                // .HasDefaultValueSql("GETDATE()")
                .IsRequired();
            
            builder.Property(o => o.Price)
                .HasColumnType("decimal(18,2)");
        }
    }
}