using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Northwind.Application.Interfaces;
using Northwind.Domain.Entities;
using Northwind.Web.Controllers;
using Northwind.Web.ViewModels.Products;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Web.UnitTests.Controllers
{
    class ProductsControllerTests
    {
        private Mocks _mocks;

        private ProductsController _controller;

        [SetUp]
        public void Setup()
        {
            _mocks = new Mocks();

            _mocks.ProductService
                .Setup(ps => ps.GetMaxAmountAsync(It.IsAny<int>()))
                .ReturnsAsync(Data.ProductEntities);

            _mocks.ProductService
                .Setup(ps => ps.GetByIdAsync(Data.ProductId))
                .ReturnsAsync(Data.ProductEntities
                    .FirstOrDefault(product => product.ProductID == Data.ProductId));

            _mocks.Configuration
                .Setup(c => c.GetSection(It.IsAny<string>()))
                .Returns(Mock.Of<IConfigurationSection>());

            _controller = new ProductsController(
                _mocks.ProductService.Object,
                _mocks.Configuration.Object,
                _mocks.Mapper.Object);
        }

        [Test]
        public async Task Index_ReturnsViewResult()
        {
            // Act
            var result = await _controller.Index();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public async Task Create_ReturnsViewResultIfHttpRequestIsGet()
        {
            // Act
            var result = await _controller.Create();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public async Task Create_ReturnsRedirectToActionIfHttpRequestIsPost()
        {
            // Act
            var result = await _controller.Create(Mock.Of<CreateProductViewModel>());

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
        }

        [Test]
        public async Task Create_ReturnsViewResultIfHttpRequestIsPostAndModelStateInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError(Data.ModelErrorKey, Data.ModelErrorMessage);

            // Act
            var result = await _controller.Create(null);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public async Task Edit_ReturnsViewResultIfHttpRequestIsGet()
        {
            // Act
            var result = await _controller.Edit(Data.ProductId);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public async Task Edit_ReturnsRedirectToActionIfHttpRequestIsPost()
        {
            // Act
            var result = await _controller.Edit(Mock.Of<EditProductViewModel>());

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
        }

        [Test]
        public void Edit_ReturnsViewResultIfHttpRequestIsPostAndModelStateInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError(Data.ModelErrorKey, Data.ModelErrorMessage);

            // Act
            var result = _controller.Edit(null).Result;

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        private static class Data
        {
            public static int ProductId { get; } = 1;

            public static ProductEntity[] ProductEntities { get; } =
                Enumerable.Range(1, 10).Select(i => new ProductEntity {ProductID = i}).ToArray();

            public static string ModelErrorKey { get; } = "test";

            public static string ModelErrorMessage { get; } = "test model";
        }

        private class Mocks
        {
            public Mock<IProductService> ProductService { get; } = new();

            public Mock<IConfiguration> Configuration { get; } = new();

            public Mock<IMapper> Mapper { get; } = new();
        }
    }
}
