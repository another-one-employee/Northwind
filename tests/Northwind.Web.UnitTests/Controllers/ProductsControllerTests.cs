using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Northwind.Core.Interfaces;
using Northwind.Core.Models;
using Northwind.Web.Controllers;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Northwind.Web.UnitTests.Controllers
{
    class ProductsControllerTests
    {
        private Mock<IProductService> _productService;
        private Mock<IAsyncRepository<SupplierDTO>> _suppliersRepository;
        private Mock<IAsyncRepository<CategoryDTO>> _cateforiessRepository;
        private static readonly int _fakeItemsCount = 10;

        [SetUp]
        public void Setup()
        {
            _productService = new Mock<IProductService>();
            _suppliersRepository = new Mock<IAsyncRepository<SupplierDTO>>();
            _cateforiessRepository = new Mock<IAsyncRepository<CategoryDTO>>();

            _productService.Setup(service => service.GetMaxAmountAsync(It.IsAny<int>())).ReturnsAsync(GetFakeItems());
        }

        private ProductsController GetProductController(int maxAmountOfProducts = 0)
        {
            var inMemorySettings = new Dictionary<string, string> {
                {"MaximumAmountOfProducts", $"{maxAmountOfProducts}"}
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            var controller = new ProductsController(
                _productService.Object,
                _suppliersRepository.Object,
                _cateforiessRepository.Object,
                configuration);

            return controller;
        }

        [Test]
        public void Index_GetView_ReturnsViewResult()
        {
            // Arrange
            var controller = GetProductController();

            // Act
            var result = controller.Index().Result;

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void Index_GetAllItems_ReturnsAllItems()
        {
            // Arrange
            var controller = GetProductController();

            // Act
            var result = controller.Index().Result as ViewResult;
            var model = result.ViewData.Model as IEnumerable<ProductDTO>;

            // Assert
            Assert.IsInstanceOf<IEnumerable<ProductDTO>>(model);
            Assert.AreEqual(GetFakeItems().Count(), model.Count());
        }

        [Test]
        public void Create_GetView_ReturnsViewResult()
        {
            // Arrange
            var controller = GetProductController();

            // Act
            var result = controller.Create().Result;

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void Create_CreateItem_ReturnsRedirectToAction()
        {
            // Arrange
            var newItem = new ProductDTO() { ProductID = 0 };
            var controller = GetProductController();

            // Act
            var result = controller.Create(newItem).Result;

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
        }

        [Test]
        public void Create_ModelStateInvalid_ReturnsViewResult()
        {
            // Arrange
            var controller = GetProductController();
            controller.ModelState.AddModelError("test", "test");

            // Act
            var result = controller.Create(null).Result;

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [TestCase(1)]
        public void Edit_GetCorrectId_ReturnsViewResult(int testId)
        {
            // Arrange
            _productService.Setup(repo => repo.GetByIdAsync(testId))
                .ReturnsAsync(GetFakeItems()
                .FirstOrDefault(product => product.ProductID == testId));

            var controller = GetProductController();

            // Act
            var result = controller.Edit(testId).Result;

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void Edit_EditItem_ReturnsRedirectToAction()
        {
            // Arrange
            var newItem = new ProductDTO() { ProductID = 0 };
            var controller = GetProductController();

            // Act
            var result = controller.Edit(newItem).Result;

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
        }

        [Test]
        public void Edit_ModelStateInvalid_ReturnsViewResult()
        {
            // Arrange
            var controller = GetProductController();
            controller.ModelState.AddModelError("test", "test");

            // Act
            var result = controller.Edit(null).Result;

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        private static IEnumerable<ProductDTO> GetFakeItems()
        {
            var categories = new List<ProductDTO>();

            for (int i = 1; i <= _fakeItemsCount; i++)
            {
                categories.Add(new ProductDTO() { ProductID = i, ProductName = $"Product {i}" });
            }

            return categories;
        }
    }
}
