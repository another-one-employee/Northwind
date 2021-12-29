using Microsoft.EntityFrameworkCore;
using Northwind.Application.Models;
using Northwind.Infrastructure.Repositories;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Infrastructure.UnitTests.Repositories
{
    public class ProductRepositoryTests
    {
        private static int _dbCounter;
        private ProductRepository _repository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ProductDbContext>()
                .UseInMemoryDatabase("TestDB-" + $"{++_dbCounter}")
                .Options;

            var context = new ProductDbContext(options);
            context.AddRange(Data.Products);
            context.SaveChanges();
            context.ChangeTracker.Clear();

            _repository = new ProductRepository(context);
        }

        [TestCase(1)]
        [TestCase(3)]
        [TestCase(10)]
        public async Task FindAsync_FindByIdNormally(int testId)
        {
            // Act
            var result = await _repository.FindAsync(x => x.ProductId == testId);

            // Assert
            Assert.AreEqual(testId, result.ProductId);
        }


        [Test]
        public async Task FindAllAsync_Normally()
        {
            // Act
            var result = await _repository.FindAllAsync();

            // Assert
            Assert.AreEqual(Data.Products.Length, result.Count());
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(3)]
        [TestCase(10)]
        public async Task TakeLast_Normally(int count)
        {
            // Act
            var result = await _repository.TakeLast(count);

            // Assert
            Assert.AreEqual(count, result.Count());
        }

        private static class Data
        {
            public static Product[] Products { get; } =
                Enumerable.Range(1, 10)
                    .Select(i => 
                        new Product
                        {
                            ProductId = i,
                            ProductName = $"Product-{i}",
                            Category = new() { CategoryId = i },
                            Supplier = new() { SupplierId = i }
                        })
                    .ToArray();
        }

        private class ProductDbContext : DbContext
        {
            public DbSet<Product> Products { get; set; }

            public ProductDbContext(DbContextOptions<ProductDbContext> options)
                : base(options) { }
        }
    }
}
