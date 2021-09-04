using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Northwind.Application.Interfaces;
using Northwind.Domain.Entities;
using Northwind.Web.Controllers;
using Northwind.Web.ViewModels.Categories;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

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
                .Setup(service => service.GetAllAsync())
                .ReturnsAsync(Data.CategoryEntities);

            _mocks.CategoryService
                .Setup(service => service.GetByIdAsync(Data.CategoryId))
                .ReturnsAsync(Data.CategoryEntities
                    .FirstOrDefault(c => c.CategoryID == Data.CategoryId));

            _mocks.CategoryService
                .Setup(service => service.EditImageById(It.IsAny<int>(), It.IsAny<byte[]>()))
                .Returns(Task.CompletedTask);

            _controller = new CategoriesController(_mocks.CategoryService.Object, _mocks.Mapper.Object);
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
        public void GetImage_ReturnsFileContentResult()
        {
            // Act
            var result = _controller.GetImage(Data.CategoryId).Result;

            // Assert
            Assert.IsInstanceOf<FileContentResult>(result);
        }

        [Test]
        public void EditImage_ReturnsViewResultIfHttpRequestIsGet()
        {
            // Act
            var result = _controller.EditImage(Data.CategoryId).Result;

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void EditImage_ReturnsRedirectToActionIfHttpRequestIsPost()
        {
            // Act
            var result = _controller.EditImage(Mock.Of<EditImageViewModel>(), Mock.Of<IFormFile>()).Result;

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
        }

        private static class Data
        {
            public static int CategoryId = 1;

            public static CategoryEntity[] CategoryEntities { get; } =
                Enumerable.Range(1, 10).Select(i => new CategoryEntity { CategoryID = i }).ToArray();
        }

        private class Mocks
        {
            public Mock<ICategoryService> CategoryService { get; } = new();

            public Mock<IMapper> Mapper { get; } = new();
        }
    }
}
