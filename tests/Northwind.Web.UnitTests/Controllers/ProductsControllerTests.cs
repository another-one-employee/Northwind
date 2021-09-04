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
                .Setup(service => service.GetMaxAmountAsync(It.IsAny<int>()))
                .ReturnsAsync(Data.ProductEntities);

            _mocks.ProductService
                .Setup(repo => repo.GetByIdAsync(Data.ProductId))
                .ReturnsAsync(Data.ProductEntities
                    .FirstOrDefault(product => product.ProductID == Data.ProductId));

            _mocks.Configuration
                .Setup(config => config.GetSection(It.IsAny<string>()))
                .Returns(Mock.Of<IConfigurationSection>());

            _controller = new(
                _mocks.ProductService.Object,
                _mocks.Configuration.Object,
                _mocks.Mapper.Object);
        }

        [Test]
        public void Index_ReturnsViewResult()
        {
            // Act
            var result = _controller.Index().Result;

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void Create_ReturnsViewResultIfHttpRequestIsGet()
        {
            // Act
            var result = _controller.Create().Result;

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void Create_ReturnsRedirectToActionIfHttpRequestIsPost()
        {
            // Act
            var result = _controller.Create(Mock.Of<CreateProductViewModel>()).Result;

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
        }

        [Test]
        public void Create_ReturnsViewResultIfHttpRequestIsPostAndModelStateInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("test", "test model");

            // Act
            var result = _controller.Create(null).Result;

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void Edit_ReturnsViewResultIfHttpRequestIsGet()
        {
            // Act
            var result = _controller.Edit(Data.ProductId).Result;

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void Edit_ReturnsRedirectToActionIfHttpRequestIsPost()
        {
            // Act
            var result = _controller.Edit(Mock.Of<EditProductViewModel>()).Result;

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
        }

        [Test]
        public void Edit_ReturnsViewResultIfHttpRequestIsPostAndModelStateInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("test", "test model");

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

        }

        private class Mocks
        {
            public Mock<IProductService> ProductService { get; } = new();

            public Mock<IConfiguration> Configuration { get; } = new();

            public Mock<IMapper> Mapper { get; } = new();
        }
    }
}
