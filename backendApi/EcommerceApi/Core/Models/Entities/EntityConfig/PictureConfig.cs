using EcommerceApi.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcommerceApi.Core.Models.Entities.EntityConfig
{
    public class PictureConfig : IEntityTypeConfiguration<Picture>
    {
        public void Configure(EntityTypeBuilder<Picture> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.AltAttribute).HasMaxLength(100);
            builder.Property(p => p.TitleAttribute).HasMaxLength(100);
        }
    }
}