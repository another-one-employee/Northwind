using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Northwind.Application.Interfaces;
using Northwind.Domain.Entities;
using Northwind.Web.Controllers;
using Northwind.Web.ViewModels.Categories;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Web.UnitTests.Controllers
{
    public class CategoriesControllerTests
    {
        private Mocks _mocks;

        private CategoriesController _controller;

        [SetUp]
        public void Setup()
        {
            _mocks = new Mocks();

            _mocks.CategoryService
                .Setup(cs => cs.GetAllAsync())
                .ReturnsAsync(Data.CategoryEntities);

            _mocks.CategoryService
                .Setup(cs => cs.GetByIdAsync(Data.CategoryId))
                .ReturnsAsync(Data.CategoryEntities
                    .FirstOrDefault(c => c.CategoryID == Data.CategoryId));

            _mocks.CategoryService
                .Setup(cs => cs.EditImageById(It.IsAny<int>(), It.IsAny<byte[]>()))
                .Returns(Task.CompletedTask);

            _controller = new CategoriesController(_mocks.CategoryService.Object, _mocks.Mapper.Object);
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
        public async Task GetImage_ReturnsFileContentResult()
        {
            // Act
            var result = await _controller.GetImage(Data.CategoryId);

            // Assert
            Assert.IsInstanceOf<FileContentResult>(result);
        }

        [Test]
        public async Task EditImage_ReturnsViewResultIfHttpRequestIsGet()
        {
            // Act
            var result = await _controller.EditImage(Data.CategoryId);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public async Task EditImage_ReturnsRedirectToActionIfHttpRequestIsPost()
        {
            // Act
            var result = await _controller.EditImage(Data.EditImageViewModel, Mock.Of<IFormFile>());

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
        }

        private static class Data
        {
            public static int CategoryId { get; } = 1;

            public static CategoryEntity[] CategoryEntities { get; } =
                Enumerable.Range(1, 10).Select(i => new CategoryEntity { CategoryID = i }).ToArray();

            public static EditImageViewModel EditImageViewModel { get; } = new()
            {
                CategoryID = 1,
                CategoryName = "test",
                Picture = new byte[1]
            };
        }

        private class Mocks
        {
            public Mock<ICategoryService> CategoryService { get; } = new();

            public Mock<IMapper> Mapper { get; } = new();
        }
    }
}
