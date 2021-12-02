using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Northwind.Application.Exceptions;
using Northwind.Application.Interfaces;
using Northwind.Application.Models;
using Northwind.Domain.Entities;
using Northwind.Web.ViewModels.Api.Categories;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using CategoriesController = Northwind.Web.Controllers.Api.CategoriesController;

namespace Northwind.Web.UnitTests.Controllers.Api
{
    public class CategoriesControllerTests
    {
        private Mocks _mocks;

        private CategoriesController _controller;

        [SetUp]
        public void SetUp()
        {
            _mocks = new Mocks();

            _mocks.CategoryService
                .Setup(cs => cs.GetAllAsync())
                .ReturnsAsync(Data.CategoryEntities);

            _mocks.CategoryService
                .Setup(cs => cs.GetPictureByIdAsync(Data.CategoryId))
                .ReturnsAsync(new byte[0]);

            _controller = new CategoriesController(_mocks.CategoryService.Object, _mocks.Mapper.Object);
        }

        [Test]
        public async Task Get_WithoutParams_ReturnsOkObjectResult()
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
            _mocks.CategoryService
                .Setup(cs => cs.GetAllAsync())
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
            var result = await _controller.Get(Data.CategoryId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task Get_WithId_ReturnsNotFoundResultIfCategoryDoesNotExist()
        {
            // Arrange
            _mocks.CategoryService
                .Setup(cs => cs.GetPictureByIdAsync(default))
                .ThrowsAsync(new NotFoundException(nameof(Category), default));

            // Act
            var result = await _controller.Get(default);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task Get_WithId_ReturnsStatus500InternalServerErrorIfExceptionWasThrow()
        {
            // Arrange
            _mocks.CategoryService
                .Setup(cs => cs.GetPictureByIdAsync(default))
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
        public async Task Put_ReturnsCategoryImageModel()
        {
            // Act
            var result = await _controller.Put(Data.CategoryImageModel);

            // Assert
            Assert.AreEqual(Data.CategoryImageModel, result.Value);
        }

        [Test]
        public async Task Put_ReturnsBadRequestResultIfCategoryImageModelIsNotValid()
        {
            // Arrange
            _controller.ModelState.AddModelError(Data.ModelErrorKey, Data.ModelErrorMessage);

            // Act
            var result = await _controller.Put(null);

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(result.Result);
        }

        [Test]
        public async Task Put_ReturnsNotFoundResultIfCategoryDoesNotExist()
        {
            // Arrange
            _mocks.CategoryService
                .Setup(cs => cs.EditImageById(It.IsAny<int>(), It.IsAny<byte[]>()))
                .ThrowsAsync(new NotFoundException(nameof(Category), default));

            // Act
            var result = await _controller.Put(Data.CategoryImageModel);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }

        
        [Test]
        public async Task Put_ReturnsStatus500InternalServerErrorIfExceptionWasThrow()
        {
            // Arrange
            _mocks.CategoryService
                .Setup(cs => cs.EditImageById(It.IsAny<int>(), It.IsAny<byte[]>()))
                .ThrowsAsync(new Exception());

            // Act
            var result = await _controller.Put(Data.CategoryImageModel);

            // Assert
            var objectResult = result.Result as ObjectResult;
            Assert.IsNotNull(objectResult);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        }

        private static class Data
        {
            public static int CategoryId { get; } = 1;

            public static CategoryEntity[] CategoryEntities { get; } =
                Enumerable.Range(1, 10).Select(i => new CategoryEntity { CategoryID = i }).ToArray();

            public static CategoryImageModel CategoryImageModel { get; } = new()
            {
                CategoryID = CategoryId,
                Picture = new byte[0]
            };

            public static string ModelErrorKey { get; } = "test";

            public static string ModelErrorMessage { get; } = "test model";
        }

        private class Mocks
        {
            public Mock<ICategoryService> CategoryService { get; } = new();

            public Mock<IMapper> Mapper { get; } = new();
        }
    }
}
