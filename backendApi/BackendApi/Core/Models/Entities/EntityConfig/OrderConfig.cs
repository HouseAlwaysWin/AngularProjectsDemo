using System;
using BackendApi.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackendApi.Core.Models.Entities.EntityConfig
{
    public class OrderConfig : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(s => s.OrderStatus)
                .HasConversion(
                    o => o.ToString(),
                    o => (OrderStatus) Enum.Parse(typeof(OrderStatus),o)
                );

            builder.HasMany(o => o.OrderItems)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
            
            
            builder.Property(o => o.TotalPrice)
                .HasColumnType("decimal(18,2)");
            
            builder.Property(o => o.CreatedDate)
                // .HasDefaultValueSql("GETDATE()")
                .IsRequired();
        }
    }
}