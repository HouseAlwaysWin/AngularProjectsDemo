using BackendApi.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackendApi.Core.Models.Entities.EntityConfig
{
    public class Product_PictureConfig : IEntityTypeConfiguration<Product_Picture>
    {
        public void Configure(EntityTypeBuilder<Product_Picture> builder)
        {
            builder.HasKey(p =>  p.Id);

            builder.HasOne(pp => pp.Product)
                .WithMany(p => p.ProductPictureMap)
                .HasForeignKey(pp => pp.ProductId);
            
            builder.HasOne(pp => pp.Picture)
                .WithMany(p => p.ProductPictureMap)
                .HasForeignKey(pp => pp.PictureId);

        }
    }
}