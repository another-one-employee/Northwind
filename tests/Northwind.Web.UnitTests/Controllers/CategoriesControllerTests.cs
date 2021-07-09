using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Northwind.Core.Interfaces;
using Northwind.Core.Models;
using Northwind.Web.Controllers;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Northwind.Web.UnitTests.Controllers
{
    public class CategoriesControllerTests
    {
        private Mock<IRepository<CategoryDTO>> mockRepo;

        [SetUp]
        public void Setup()
        {
            mockRepo = new Mock<IRepository<CategoryDTO>>();
        }

        [Test]
        public void Index_GetView_ReturnsViewResult()
        {
            // Arrange
            var controller = new CategoriesController(mockRepo.Object);

            // Act
            var result = controller.Index().Result;

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void Index_GetAllItems_ReturnsAllItems()
        {
            // Arrange
            mockRepo.Setup(repo => repo.FindAllAync())
                .ReturnsAsync(GetFakeItems());
            var controller = new CategoriesController(mockRepo.Object);

            // Act
            var result = controller.Index().Result as ViewResult;
            var model = result.ViewData.Model as IEnumerable<CategoryDTO>;

            // Assert
            Assert.IsInstanceOf<IEnumerable<CategoryDTO>>(model);
            Assert.AreEqual(GetFakeItems().Count(), model.Count());

        }

        [TestCase(1)]
        public void GetImage_GetItem_ReturnsFileContentResult(int testId)
        {
            // Arrange
            mockRepo.Setup(repo => repo.FindAync(testId))
                .ReturnsAsync(GetFakeItems()
                .FirstOrDefault(c => c.CategoryID == testId));
            var controller = new CategoriesController(mockRepo.Object);

            // Act
            var result = controller.GetImage(testId).Result;

            // Assert
            Assert.IsInstanceOf<FileContentResult>(result);
        }

        [Test]
        public void GetImage_GetUnexistedItem_ReturnsNotFoundResult()
        {
            // Arrange
            var controller = new CategoriesController(mockRepo.Object);

            // Act
            var result = controller.GetImage(0).Result;

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [TestCase(1)]
        public void EditImage_GetItem_ReturnsViewResult(int testId)
        {
            // Arrange
            mockRepo.Setup(repo => repo.FindAync(testId))
                .ReturnsAsync(GetFakeItems()
                .FirstOrDefault(c => c.CategoryID == testId));
            var controller = new CategoriesController(mockRepo.Object);

            // Act
            var result = controller.EditImage(testId).Result;

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void EditImage_GetUnexistedItem_ReturnsNotFoundResult()
        {
            // Arrange
            var controller = new CategoriesController(mockRepo.Object);

            // Act
            var result = controller.EditImage(0).Result;

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public void EditImage_PostRequest_ReturnsRedirectToAction()
        {
            // Arrange
            var testItem = new Mock<CategoryDTO>();
            var testFile = new Mock<IFormFile>();

            mockRepo.Setup(repo => repo.Update(testItem.Object));
            var controller = new CategoriesController(mockRepo.Object);

            // Act
            var result = controller.EditImage(testItem.Object, testFile.Object).Result;

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
        }

        private static IEnumerable<CategoryDTO> GetFakeItems()
        {
            var categories = new List<CategoryDTO>
            {
                new CategoryDTO()
                {
                    CategoryID = 1,
                    CategoryName = "Cetagory 1",
                    Description = "Testing category 1",
                    Picture = new byte[] { 1 }
                },
                new CategoryDTO()
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
