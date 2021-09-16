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
    public class CategoryServiceTests
    {
        private Mocks _mocks;
        private CategoryService _service;

        [SetUp]
        public void SetUp()
        {
            _mocks = new();

            _mocks.CategoryRepository
                .Setup(cr => cr.FindAllAsync())
                .ReturnsAsync(Data.Categories);

            _mocks.Mapper
                .Setup(m => m.Map<IEnumerable<CategoryEntity>>(It.IsAny<IEnumerable<Category>>()))
                .Returns(Data.CategoryEntities);

            _service = new(_mocks.CategoryRepository.Object, _mocks.Mapper.Object);
        }

        [Test]
        public async Task GetAllAsync_ReturnsAllMappedItems()
        {
            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.AreEqual(Data.CategoriesCount, result.Count());
        }

        [TestCase(1)]
        [TestCase(3)]
        [TestCase(10)]
        public async Task GetByIdAsync_ReturnsMappedItemWithCorrectId(int id)
        {
            // Arrange
            _mocks.CategoryRepository
                .Setup(cr => cr.FindAsync(c => c.CategoryId == id))
                .ReturnsAsync(new Category { CategoryId = id });

            _mocks.Mapper
                .Setup(m => m.Map<CategoryEntity>(It.IsAny<Category>()))
                .Returns(new CategoryEntity { CategoryID = id });

            // Act
            var result = await _service.GetByIdAsync(id);

            // Assert
            Assert.AreEqual(id, result.CategoryID);
        }

        [Test]
        public async Task GetByIdAsync_ThrowsNotFoundException()
        {
            // Arrange
            _mocks.CategoryRepository
                .Setup(cr => cr.FindAsync(It.IsAny<Expression<Func<Category, bool>>>()));

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

        [TestCase(null, 2)]
        [TestCase(new byte[0], 6)]
        [TestCase(new byte[] { 1, 7, 7, 0, 1, 3, 3 }, 10)]
        public async Task GetPictureByIdAsync_ReturnsCorrectPicture(byte[] picture, int id)
        {
            // Arrange
            _mocks.CategoryRepository
                .Setup(cr => cr.FindAsync(c => c.CategoryId == id))
                .ReturnsAsync(new Category { CategoryId = id, Picture = picture });

            _mocks.Mapper
                .Setup(m => m.Map<CategoryEntity>(It.IsAny<Category>()))
                .Returns(new CategoryEntity { CategoryID = id, Picture = picture });

            // Act
            var result = await _service.GetPictureByIdAsync(id);

            // Assert
            Assert.AreEqual(picture, result);
        }

        [Test]
        public async Task EditImageById_Normally()
        {
            // Arrange
            _mocks.CategoryRepository
                .Setup(cr => cr.FindAsync(It.IsAny<Expression<Func<Category, bool>>>()))
                .ReturnsAsync(new Category());

            // Act
            await _service.EditImageById(default, default);

            // Assert
            _mocks.CategoryRepository.Verify(cr => cr.UpdateAsync(It.IsAny<Category>()), Times.Once);
        }

        private static class Data
        {
            public static int CategoriesCount { get; } = 10;

            public static Category[] Categories { get; } =
                Enumerable.Range(1, CategoriesCount).Select(i => new Category { CategoryId = i }).ToArray();

            public static CategoryEntity[] CategoryEntities { get; } =
                Enumerable.Range(1, CategoriesCount).Select(i => new CategoryEntity { CategoryID = i }).ToArray();
        }

        private class Mocks
        {
            public Mock<IAsyncRepository<Category>> CategoryRepository { get; } = new();

            public Mock<IMapper> Mapper { get; } = new();
        }
    }
}