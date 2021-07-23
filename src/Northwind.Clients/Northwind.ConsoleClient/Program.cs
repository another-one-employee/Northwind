using Northwind.ConsoleClient.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Northwind.ConsoleClient
{
    static class Program
    {
        static readonly HttpClient client = new();

        static void Main()
        {
            RunAsync().GetAwaiter().GetResult();
        }

        static async Task RunAsync()
        {
            client.BaseAddress = new Uri("http://localhost:5000/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

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
            HttpResponseMessage response = await client.GetAsync("api/products");

            if (response.IsSuccessStatusCode)
            {
                var products = await response.Content.ReadAsAsync<IEnumerable<Product>>();
                return products;
            }

            return null;
        }

        static async Task<IEnumerable<Category>> TryGetCategoriesAsync()
        {
            HttpResponseMessage response = await client.GetAsync("api/categories");

            if (response.IsSuccessStatusCode)
            {
                var categories = await response.Content.ReadAsAsync<IEnumerable<Category>>();
                return categories;
            }

            return null;
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
                $"  {category.Description, 10}\n");
        }
    }
}
