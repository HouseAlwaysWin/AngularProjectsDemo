using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EcommerceApi.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EcommerceApi.Core.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext context,ILoggerFactory loggerFactory){
            try{
                using(var trans = context.Database.BeginTransaction()){

                    if(!context.ProductCategories.Any()){
                        var productCategoriesData = 
                            File.ReadAllText("./Core/Data/SeedData/categories.json");
                        
                        var categories = JsonConvert.DeserializeObject<List<ProductCategory>>(productCategoriesData);

                        foreach (var item in categories)
                        {
                           context.ProductCategories.Add(item);
                        }
                        await context.Database.ExecuteSqlRawAsync ("SET IDENTITY_INSERT dbo.ProductCategories ON");
                        await context.SaveChangesAsync();
                        await context.Database.ExecuteSqlRawAsync ("SET IDENTITY_INSERT dbo.ProductCategories OFF");
                    }

                    if(!context.ProductBrands.Any()){
                        var productBrandsData = 
                            File.ReadAllText("./Core/Data/SeedData/brands.json");
                        
                        var brands = JsonConvert.DeserializeObject<List<ProductBrand>>(productBrandsData);

                        foreach (var item in brands)
                        {
                           context.ProductBrands.Add(item);
                        }
                        await context.Database.ExecuteSqlRawAsync ("SET IDENTITY_INSERT dbo.ProductBrands ON");
                        await context.SaveChangesAsync();
                        await context.Database.ExecuteSqlRawAsync ("SET IDENTITY_INSERT dbo.ProductBrands OFF");
                    }

                     if(!context.DeliveryMethods.Any()){
                        var deliveryMethodData = 
                            File.ReadAllText("./Core/Data/SeedData/delivery.json");
                        
                        var deliveries = JsonConvert.DeserializeObject<List<DeliveryMethod>>(deliveryMethodData);

                        foreach (var item in deliveries)
                        {
                           context.DeliveryMethods.Add(item);
                        }
                        await context.Database.ExecuteSqlRawAsync ("SET IDENTITY_INSERT dbo.DeliveryMethods ON");
                        await context.SaveChangesAsync();
                        await context.Database.ExecuteSqlRawAsync ("SET IDENTITY_INSERT dbo.DeliveryMethods OFF");
                    }

                    if(!context.Products.Any()){
                        var productData = 
                            File.ReadAllText("./Core/Data/SeedData/products.json");
                        
                        var products = JsonConvert.DeserializeObject<List<Product>>(productData);

                        foreach (var item in products)
                        {
                           context.Products.Add(item);
                        }
                        // await context.Database.ExecuteSqlRawAsync ("SET IDENTITY_INSERT dbo.Products ON");
                        await context.SaveChangesAsync();
                        // await context.Database.ExecuteSqlRawAsync ("SET IDENTITY_INSERT dbo.Products OFF");
                    }

                    await trans.CommitAsync();
                }
            }catch(Exception ex){
                var logger = loggerFactory.CreateLogger<StoreContext>();
                logger.LogError(ex.Message);
            }
        }
    }
}