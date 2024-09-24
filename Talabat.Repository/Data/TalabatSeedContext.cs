using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Repository.Data
{
    public class TalabatSeedContext
    {
        public static async Task SeedAsync(TalabatContext context , ILoggerFactory loggerFactory)
        {

            try
            {
                if (!context.ProductBrands.Any())
                {
                    var brandsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/brands.json");
                    var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
                    foreach (var brand in brands)
                        context.Set<ProductBrand>().Add(brand);
                }

                if (!context.ProductTypes.Any())
                {
                    var typesData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/types.json");
                    var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);
                    foreach (var type in types)
                        context.Set<ProductType>().Add(type);
                }

                if (!context.Products.Any())
                {
                    var productsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/products.json");
                    var products = JsonSerializer.Deserialize<List<Product>>(productsData);
                    foreach (var product in products)
                        context.Set<Product>().Add(product);
                }

                if (!context.DeliveryMethods.Any())
                {
                    var deliveryMethodsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/delivery.json");
                    var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryMethodsData);
                    foreach (var deliveryMethod in deliveryMethods)
                        context.Set<DeliveryMethod>().Add(deliveryMethod);
                }

                await context.SaveChangesAsync();

            }
            catch (Exception ex) 
            {
                var logger = loggerFactory.CreateLogger<TalabatSeedContext>();
                logger.LogError(ex , ex.Message);
            }

        }
    }
}
