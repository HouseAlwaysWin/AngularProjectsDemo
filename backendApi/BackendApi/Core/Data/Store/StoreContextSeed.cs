using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BackendApi.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BackendApi.Core.Data.Store
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
                        // await context.Database.ExecuteSqlRawAsync ("SET IDENTITY_INSERT dbo.ProductCategories ON");
                        await context.SaveChangesAsync();
                        // await context.Database.ExecuteSqlRawAsync ("SET IDENTITY_INSERT dbo.ProductCategories OFF");
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

                
                    if(!context.DeliveryMethods.Any()){
                        var deliveryMethodData = 
                            File.ReadAllText("./Core/Data/SeedData/delivery.json");
                        
                        var deliveries = JsonConvert.DeserializeObject<List<DeliveryMethod>>(deliveryMethodData);

                        foreach (var item in deliveries)
                        {
                           context.DeliveryMethods.Add(item);
                        }
                        // await context.Database.ExecuteSqlRawAsync ("SET IDENTITY_INSERT dbo.DeliveryMethods ON");
                        await context.SaveChangesAsync();
                        // await context.Database.ExecuteSqlRawAsync ("SET IDENTITY_INSERT dbo.DeliveryMethods OFF");
                    }

                    if(!context.Language.Any()){
                        var languageData = 
                            File.ReadAllText("./Core/Data/SeedData/language.json");
                        
                        var languages = JsonConvert.DeserializeObject<List<Language>>(languageData);

                        foreach (var item in languages)
                        {
                           context.Language.Add(item);
                        }
                        // await context.Database.ExecuteSqlRawAsync ("SET IDENTITY_INSERT dbo.Language ON");
                        await context.SaveChangesAsync();
                        // await context.Database.ExecuteSqlRawAsync ("SET IDENTITY_INSERT dbo.Language OFF");
                    }


                     if(!context.Localized.Any()){
                        var localizedData = 
                            File.ReadAllText("./Core/Data/SeedData/localize.json");
                        
                        var localizeds = JsonConvert.DeserializeObject<List<Localized>>(localizedData);

                        foreach (var item in localizeds)
                        {
                           context.Localized.Add(item);
                        }
                        // await context.Database.ExecuteSqlRawAsync ("SET IDENTITY_INSERT dbo.Localized ON");
                        await context.SaveChangesAsync();
                        // await context.Database.ExecuteSqlRawAsync ("SET IDENTITY_INSERT dbo.Localized OFF");
                    }

                     if(!context.Pictures.Any()){
                        var pictureData = 
                            File.ReadAllText("./Core/Data/SeedData/picture.json");
                        
                        var pictures = JsonConvert.DeserializeObject<List<Picture>>(pictureData);

                        foreach (var item in pictures)
                        {
                           context.Pictures.Add(item);
                        }
                        // await context.Database.ExecuteSqlRawAsync ("SET IDENTITY_INSERT dbo.Pictures ON");
                        await context.SaveChangesAsync();
                        // await context.Database.ExecuteSqlRawAsync ("SET IDENTITY_INSERT dbo.Pictures OFF");
                    }


                     if(!context.ProductAttributes.Any()){
                        var paData = 
                            File.ReadAllText("./Core/Data/SeedData/productAttribute.json");
                        
                        var productAttributes = JsonConvert.DeserializeObject<List<ProductAttribute>>(paData);

                        foreach (var item in productAttributes)
                        {
                           context.ProductAttributes.Add(item);
                        }
                        // await context.Database.ExecuteSqlRawAsync ("SET IDENTITY_INSERT dbo.ProductAttributes ON");
                        await context.SaveChangesAsync();
                        // await context.Database.ExecuteSqlRawAsync ("SET IDENTITY_INSERT dbo.ProductAttributes OFF");
                    }


                     if(!context.ProductAttributeValues.Any()){
                        var paData = 
                            File.ReadAllText("./Core/Data/SeedData/productAttributeValue.json");
                        
                        var productAttributeValues = JsonConvert.DeserializeObject<List<ProductAttributeValue>>(paData);

                        foreach (var item in productAttributeValues)
                        {
                           context.ProductAttributeValues.Add(item);
                        }
                        // await context.Database.ExecuteSqlRawAsync ("SET IDENTITY_INSERT dbo.ProductAttributeValues ON");
                        await context.SaveChangesAsync();
                        // await context.Database.ExecuteSqlRawAsync ("SET IDENTITY_INSERT dbo.ProductAttributeValues OFF");
                    }

              


                  


                    if(!context.Product_Pictures.Any()){
                        var productpictureData = 
                            File.ReadAllText("./Core/Data/SeedData/product_picture.json");
                        
                        var productpictures = JsonConvert.DeserializeObject<List<Product_Picture>>(productpictureData);

                        foreach (var item in productpictures)
                        {
                           context.Product_Pictures.Add(item);
                        }
                        // await context.Database.ExecuteSqlRawAsync ("SET IDENTITY_INSERT dbo.Product_Pictures ON");
                        await context.SaveChangesAsync();
                        // await context.Database.ExecuteSqlRawAsync ("SET IDENTITY_INSERT dbo.Product_Pictures OFF");
                    }



                    if(!context.Product_ProductAttributes.Any()){
                        var product_productAttrData = 
                            File.ReadAllText("./Core/Data/SeedData/product_productAttribute.json");
                        
                        var product_productAttrs = JsonConvert.DeserializeObject<List<Product_ProductAttribute>>(product_productAttrData);

                        foreach (var item in product_productAttrs)
                        {
                           context.Product_ProductAttributes.Add(item);
                        }
                        // await context.Database.ExecuteSqlRawAsync ("SET IDENTITY_INSERT dbo.Product_ProductAttributes ON");
                        await context.SaveChangesAsync();
                        // await context.Database.ExecuteSqlRawAsync ("SET IDENTITY_INSERT dbo.Product_ProductAttributes OFF");
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