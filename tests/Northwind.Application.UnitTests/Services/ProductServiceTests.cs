using AutoMapper;
using Moq;
using Northwind.Application.Exceptions;
using Northwind.Application.Interfaces;
using Northwind.Application.Models;
using Northwind.Application.Services;
using Northwind.Domain.Entities;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Northwind.Application.UnitTests.Services
{
    public class ProductServiceTests
    {
        private Mocks _mocks;

        private ProductService _service;

        public static int[] ProductsCountsCaseSource { get; } = { 1, 3, 10 };

        [SetUp]
        public void SetUp()
        {
            _mocks = new();

            _service = new(
                _mocks.ProductAsyncRepository.Object,
                _mocks.SupplierRepository.Object,
                _mocks.CategoryRepository.Object,
                _mocks.Mapper.Object);
        }

        [Test]
        public async Task GetMaxAmountAsync_ReturnsAllMappedItemsIfMaxAmountOfProductsEqual0()
        {
            // Arrange
            _mocks.ProductAsyncRepository
                .Setup(pr => pr.FindAllAsync())
                .ReturnsAsync(Data.Products);

            _mocks.Mapper
                .Setup(m => m.Map<IEnumerable<ProductEntity>>(It.IsAny<IEnumerable<Product>>()))
                .Returns(Data.ProductEntities);

            // Act
            var result = await _service.GetMaxAmountAsync(0);

            // Assert
            Assert.AreEqual(Data.ProductsCount, result.Count());
        }

        [TestCaseSource(nameof(ProductsCountsCaseSource))]
        public async Task GetMaxAmountAsync_ReturnsCorrectCountOfMaxAmountOfProducts(int maxAmountOfProducts)
        {
            // Arrange
            _mocks.ProductAsyncRepository
                .Setup(pr => pr.TakeLast(maxAmountOfProducts))
                .ReturnsAsync(Enumerable.Range(1, maxAmountOfProducts).Select(i => new Product { ProductId = i }));

            _mocks.Mapper
                .Setup(m => m.Map<IEnumerable<ProductEntity>>(It.IsAny<IEnumerable<Product>>()))
                .Returns(Enumerable.Range(1, maxAmountOfProducts).Select(i => new ProductEntity { ProductID = i }));

            // Act
            var result = await _service.GetMaxAmountAsync(maxAmountOfProducts);

            // Assert
            Assert.AreEqual(maxAmountOfProducts, result.Count());
        }

        [Test]
        public async Task GetCategories_ReturnsAllMappedItems()
        {
            // Arrange
            _mocks.CategoryRepository
                .Setup(cr => cr.FindAllAsync())
                .ReturnsAsync(Data.Categories);

            _mocks.Mapper
                .Setup(m => m.Map<IEnumerable<CategoryEntity>>(It.IsAny<IEnumerable<Category>>()))
                .Returns(Data.CategoryEntities);

            // Act
            var result = await _service.GetCategories();

            // Assert
            Assert.AreEqual(Data.CategoriesCount, result.Count());
        }

        [Test]
        public async Task GetSuppliers_ReturnsAllMappedItems()
        {
            // Arrange
            _mocks.SupplierRepository
                .Setup(sr => sr.FindAllAsync())
                .ReturnsAsync(Data.Suppliers);

            _mocks.Mapper
                .Setup(m => m.Map<IEnumerable<SupplierEntity>>(It.IsAny<IEnumerable<Supplier>>()))
                .Returns(Data.SupplierEntities);

            // Act
            var result = await _service.GetSuppliers();

            // Assert
            Assert.AreEqual(Data.SuppliersCount, result.Count());
        }

        [Test]
        public async Task CreateAsync_Normally()
        {
            // Act
            await _service.CreateAsync(Mock.Of<ProductEntity>());

            // Assert
            _mocks.ProductAsyncRepository.Verify(pr => pr.InsertAsync(It.IsAny<Product>()), Times.Once);
        }

        [Test]
        public async Task UpdateAsync_Normally()
        {
            // Act
            await _service.UpdateAsync(Mock.Of<ProductEntity>());

            // Assert
            _mocks.ProductAsyncRepository.Verify(pr => pr.UpdateAsync(It.IsAny<Product>()), Times.Once);
        }

        [Test]
        public async Task DeleteAsync_Normally()
        {
            // Act
            await _service.DeleteAsync(Mock.Of<ProductEntity>());

            // Assert
            _mocks.ProductAsyncRepository.Verify(pr => pr.DeleteAsync(It.IsAny<Product>()), Times.Once);
        }

        [TestCase(1)]
        [TestCase(3)]
        [TestCase(10)]
        public async Task GetByIdAsync_ReturnsMappedItemWithCorrectId(int id)
        {
            // Arrange
            _mocks.ProductAsyncRepository
                .Setup(pr => pr.FindAsync(p => p.ProductId == id))
                .ReturnsAsync(new Product { ProductId = id });

            _mocks.Mapper
                .Setup(m => m.Map<ProductEntity>(It.IsAny<Product>()))
                .Returns(new ProductEntity { ProductID = id });

            // Act
            var result = await _service.GetByIdAsync(id);

            // Assert
            Assert.AreEqual(id, result.ProductID);
        }

        [Test]
        public async Task GetByIdAsync_ThrowsNotFoundException()
        {
            // Arrange
            _mocks.ProductAsyncRepository
                .Setup(pr => pr.FindAsync(It.IsAny<Expression<Func<Product, bool>>>()));

            // Act
            try
            {
                await _service.GetByIdAsync(default);

                // Assert
                Assert.Fail();
            }
            // Assert
            catch (NotFoundException)
            {
                Assert.Pass();
            }
        }

        private static class Data
        {
            public static int ProductsCount { get; } = 10;

            public static int CategoriesCount { get; } = 10;

            public static int SuppliersCount { get; } = 10;

            public static Product[] Products { get; } =
                Enumerable.Range(1, ProductsCount).Select(i => new Product { ProductId = i }).ToArray();

            public static ProductEntity[] ProductEntities { get; } =
                Enumerable.Range(1, ProductsCount).Select(i => new ProductEntity { ProductID = i }).ToArray();

            public static Category[] Categories { get; } =
                Enumerable.Range(1, CategoriesCount).Select(i => new Category { CategoryId = i }).ToArray();

            public static CategoryEntity[] CategoryEntities { get; } =
                Enumerable.Range(1, CategoriesCount).Select(i => new CategoryEntity { CategoryID = i }).ToArray();

            public static Supplier[] Suppliers { get; } =
                Enumerable.Range(1, SuppliersCount).Select(i => new Supplier { SupplierId = i, CompanyName = $"Company-{i}" }).ToArray();

            public static SupplierEntity[] SupplierEntities { get; } =
                Enumerable.Range(1, SuppliersCount).Select(i => new SupplierEntity() { SupplierID = i, CompanyName = $"Company-{i}" }).ToArray();
        }

        private class Mocks
        {
            public Mock<IProductAsyncRepository> ProductAsyncRepository { get; } = new();

            public Mock<IAsyncRepository<Supplier>> SupplierRepository { get; } = new();

            public Mock<IAsyncRepository<Category>> CategoryRepository { get; } = new();

            public Mock<IMapper> Mapper { get; } = new();
        }
    }
}
