using IO.Swagger.Api;
using Newtonsoft.Json;
using Northwind.ConsoleClient.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Northwind.ConsoleClient
{
    static class Program
    {
        static readonly string BasePath = "http://localhost:5000/";

        static async Task Main()
        {
            try
            {
                var categories = await TryGetCategoriesAsync();

                foreach (var c in categories)
                {
                    ShowCategories(c);
                }

                var products = await TryGetProductsAsync();

                foreach (var p in products)
                {
                    ShowProduct(p);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadKey();
        }

        static async Task<IEnumerable<Product>> TryGetProductsAsync()
        {
            var client = new ProductsApi(BasePath);
            var response = await client.ApiProductsGetAsyncWithHttpInfo();

            IEnumerable<Product> products = null;

            if (response != null)
            {
                products = JsonConvert.DeserializeObject<IEnumerable<Product>>((string)response.Data);
            }

            return products;
        }

        static async Task<IEnumerable<Category>> TryGetCategoriesAsync()
        {
            var client = new CategoriesApi(BasePath);
            var response = await client.ApiCategoriesGetAsyncWithHttpInfo();

            IEnumerable<Category> categories = null;

            if (response != null)
            {
                categories = JsonConvert.DeserializeObject<IEnumerable<Category>>((string)response.Data);
            }

            return categories;
        }

        static void ShowProduct(Product product)
        {
            Console.WriteLine($"Product: {product.ProductName}\n" +
                $"  Quantity per unit: {product.QuantityPerUnit}; Price: {product.UnitPrice}\n" +
                $"      Supplier: {product.Supplier.CompanyName}; Category: {product.Category.CategoryName}\n");
        }

        static void ShowCategories(Category category)
        {
            Console.WriteLine($"Category: {category.CategoryName}\n" +
                $"  {category.Description,10}\n");
        }
    }
}
