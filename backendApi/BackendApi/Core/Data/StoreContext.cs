using System.Reflection;
using BackendApi.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BackendApi.Core.Data
{
    public class StoreContext:DbContext
    {

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductAttribute> ProductAttributes { get; set; }
        public DbSet<ProductAttributeValue> ProductAttributeValues { get; set; }
        public DbSet<Product_ProductAttribute> Product_ProductAttributes { get; set; }
        public DbSet<Product_Picture> Product_Pictures { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<Language> Language { get; set; }
        public DbSet<Localized> Localized { get; set; }

        public DbSet<Order> Orders {get;set;}
        public DbSet<OrderItem> OrderItems {get;set;}
        public DbSet<DeliveryMethod> DeliveryMethods {get;set;}
        public DbSet<OrderAddress> OrderAddresses {get;set;}

        public StoreContext(DbContextOptions<StoreContext> options):base (options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder){

            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly (Assembly.GetExecutingAssembly ());
        }
        
    }
}