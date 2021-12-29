using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Northwind.Application.Exceptions;
using Northwind.Application.Interfaces;
using Northwind.Domain.Entities;
using Northwind.Web.Controllers.Api;
using Northwind.Web.ViewModels.Api.Products;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Northwind.Web.UnitTests.Controllers.Api
{
    public class ProductsControllerTests
    {
        private Mocks _mocks;

        private ProductsController _controller;

        [SetUp]
        public void Setup()
        {
            _mocks = new Mocks();

            _mocks.Configuration
                .Setup(c => c.GetSection(It.IsAny<string>()))
                .Returns(Mock.Of<IConfigurationSection>());

            _controller = new ProductsController(
                _mocks.ProductService.Object,
                _mocks.Configuration.Object,
                _mocks.Mapper.Object);
        }

        [Test]
        public async Task Get_WithoutParamsReturnsOkObjectResult()
        {
            // Act
            var result = await _controller.Get();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task Get_WithoutParams_ReturnsStatus500InternalServerErrorIfExceptionWasThrow()
        {
            // Arrange
            _mocks.ProductService
                .Setup(ps => ps.GetMaxAmountAsync(It.IsAny<int>()))
                .ThrowsAsync(new Exception());

            // Act
            var result = await _controller.Get();

            // Assert
            Assert.IsInstanceOf<ObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.IsNotNull(objectResult);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        }

        [Test]
        public async Task Get_WithId_ReturnsOkObjectResult()
        {
            // Act
            var result = await _controller.Get(It.IsAny<int>());

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task Get_WithId_ReturnsNotFoundResultIfProductDoesNotExist()
        {
            // Arrange
            _mocks.ProductService
                .Setup(ps => ps.GetByIdAsync(default))
                .ThrowsAsync(new NotFoundException(nameof(ProductEntity), default));

            // Act
            var result = await _controller.Get(default);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task Get_WithId_ReturnsStatus500InternalServerErrorIfExceptionWasThrow()
        {
            // Arrange
            _mocks.ProductService
                .Setup(ps => ps.GetByIdAsync(It.IsAny<int>()))
                .ThrowsAsync(new Exception());

            // Act
            var result = await _controller.Get(default);

            // Assert
            Assert.IsInstanceOf<ObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.IsNotNull(objectResult);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        }

        [Test]
        public async Task Post_ReturnsCreatedResult()
        {
            // Arrange
            _mocks.Mapper
                .Setup(m => m.Map<ProductEntity>(It.IsAny<CreateProductModel>()))
                .Returns(new ProductEntity());

            // Act
            var result = await _controller.Post(Data.CreateProductModel);

            // Assert
            Assert.IsInstanceOf<CreatedResult>(result.Result);
        }

        
        [Test]
        public async Task Post_ReturnsBadRequestResultIfCreateProductModelIsNotValid()
        {
            // Arrange
            _controller.ModelState.AddModelError(Data.ModelErrorKey, Data.ModelErrorMessage);

            // Act
            var result = await _controller.Post(null);

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(result.Result);
        }

                
        [Test]
        public async Task Post_ReturnsStatus500InternalServerErrorIfExceptionWasThrow()
        {
            // Arrange
            _mocks.ProductService
                .Setup(ps => ps.GetByIdAsync(It.IsAny<int>()))
                .ThrowsAsync(new Exception());

            // Act
            var result = await _controller.Post(Data.CreateProductModel);

            // Assert
            var objectResult = result.Result as ObjectResult;
            Assert.IsNotNull(objectResult);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        }

         
        [Test]
        public async Task Put_ReturnsUpdateProductModel()
        {
            // Arrange
            _mocks.Mapper
                .Setup(m => m.Map<ProductEntity>(It.IsAny<UpdateProductModel>()))
                .Returns(new ProductEntity());

            // Act
            var result = await _controller.Put(Data.UpdateProductModel);

            // Assert
            Assert.AreEqual(Data.UpdateProductModel, result.Value);
        }

        
        [Test]
        public async Task Put_ReturnsBadRequestResultIfUpdateProductModelIsNotValid()
        {
            // Arrange
            _controller.ModelState.AddModelError(Data.ModelErrorKey, Data.ModelErrorMessage);

            // Act
            var result = await _controller.Put(null);

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(result.Result);
        }

        [Test]
        public async Task Put_ReturnsNotFoundResultIfProductDoesNotExist()
        {
            // Arrange
            _mocks.ProductService
                .Setup(ps => ps.UpdateAsync(It.IsAny<ProductEntity>()))
                .ThrowsAsync(new NotFoundException(nameof(ProductEntity), default));

            // Act
            var result = await _controller.Put(Data.UpdateProductModel);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }

        [Test]
        public async Task Put_ReturnsStatus500InternalServerErrorIfExceptionWasThrow()
        {
            // Arrange
            _mocks.ProductService
                .Setup(ps => ps.UpdateAsync(It.IsAny<ProductEntity>()))
                .ThrowsAsync(new Exception());

            // Act
            var result = await _controller.Put(Data.UpdateProductModel);

            // Assert
            var objectResult = result.Result as ObjectResult;
            Assert.IsNotNull(objectResult);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        }

        [Test]
        public async Task Delete_ReturnsOkObjectResult()
        {
            // Act
            var result = await _controller.Delete(Data.ProductId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(int.MinValue)]
        public async Task Delete_ReturnsBadRequestResultIfIdIsNotValid(int id)
        {
            // Act
            var result = await _controller.Delete(id);

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public async Task Delete_ReturnsNotFoundResultIfProductDoesNotExist()
        {
            // Arrange
            _mocks.ProductService
                .Setup(ps => ps.GetByIdAsync(It.IsAny<int>()))
                .ThrowsAsync(new NotFoundException(nameof(ProductEntity), default));

            // Act
            var result = await _controller.Delete(Data.ProductId);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task Delete_ReturnsStatus500InternalServerErrorIfExceptionWasThrow()
        {
            // Arrange
            _mocks.ProductService
                .Setup(ps => ps.GetByIdAsync(It.IsAny<int>()))
                .ThrowsAsync(new Exception());

            // Act
            var result = await _controller.Delete(Data.ProductId);

            // Assert
            Assert.IsInstanceOf<ObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.IsNotNull(objectResult);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        }

        private static class Data
        {
            public static int ProductId { get; } = 1;

            public static CreateProductModel CreateProductModel { get; } = new()
            {
                ProductName = "Test"
            };

            public static UpdateProductModel UpdateProductModel { get; } = new()
            {
                ProductID = 1,
                ProductName = "Test"
            };

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
