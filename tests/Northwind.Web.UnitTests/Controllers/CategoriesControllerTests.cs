using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Northwind.Application.Interfaces;
using Northwind.Domain.Entities;
using Northwind.Web.Controllers;
using Northwind.Web.ViewModels.Categories;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Northwind.Web.UnitTests.Controllers
{
    public class CategoriesControllerTests
    {
        private Mock<ICategoryService> _mockService;
        private Mock<IMapper> _mapper;

        private CategoriesController GetCategoriesController()
            => new CategoriesController(_mockService.Object, _mapper.Object);

        [SetUp]
        public void Setup()
        {
            _mockService = new Mock<ICategoryService>();
            _mapper = new Mock<IMapper>();
        }

        [Test]
        public void Index_GetView_ReturnsViewResult()
        {
            // Arrange
            var controller = GetCategoriesController();

            // Act
            var result = controller.Index().Result;

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void Index_GetAllItems_ReturnsAllItems()
        {
            // Arrange
            _mockService.Setup(service => service.GetAllAsync())
                .ReturnsAsync(GetFakeItems());
            var controller = GetCategoriesController();

            // Act
            var result = controller.Index().Result as ViewResult;
            var model = result.ViewData.Model as IEnumerable<CategoryEntity>;

            // Assert
            Assert.IsInstanceOf<IEnumerable<CategoryEntity>>(model);
            Assert.AreEqual(GetFakeItems().Count(), model.Count());

        }

        [TestCase(1)]
        public void GetImage_GetItem_ReturnsFileContentResult(int testId)
        {
            // Arrange
            _mockService.Setup(service => service.GetByIdAsync(testId))
                .ReturnsAsync(GetFakeItems()
                .FirstOrDefault(c => c.CategoryID == testId));
            var controller = GetCategoriesController();

            // Act
            var result = controller.GetImage(testId).Result;

            // Assert
            Assert.IsInstanceOf<FileContentResult>(result);
        }

        [TestCase(1)]
        public void EditImage_GetItem_ReturnsViewResult(int testId)
        {
            // Arrange
            _mockService.Setup(service => service.GetByIdAsync(testId))
                .ReturnsAsync(GetFakeItems()
                .FirstOrDefault(c => c.CategoryID == testId));
            var controller = GetCategoriesController();

            // Act
            var result = controller.EditImage(testId).Result;

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void EditImage_PostRequest_ReturnsRedirectToAction()
        {
            // Arrange
            var testItem = new Mock<EditImageViewModel>();
            var testFile = new Mock<IFormFile>();

            _mockService.Setup(service => service.EditImageById(It.IsAny<int>(), It.IsAny<byte[]>()));
            var controller = GetCategoriesController();

            // Act
            var result = controller.EditImage(testItem.Object, testFile.Object).Result;

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
        }

        private static IEnumerable<CategoryEntity> GetFakeItems()
        {
            var categories = new List<CategoryEntity>
            {
                new CategoryEntity()
                {
                    CategoryID = 1,
                    CategoryName = "Cetagory 1",
                    Description = "Testing category 1",
                    Picture = new byte[] { 1 }
                },
                new CategoryEntity()
                {
                    CategoryID = 2,
                    CategoryName = "Cetagory 2",
                    Description = "Testing category 2",
                    Picture = new byte[] { 2 }
                }
            };
            return categories;
        }
    }
}
