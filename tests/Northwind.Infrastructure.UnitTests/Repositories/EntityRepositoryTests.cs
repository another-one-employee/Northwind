using Microsoft.EntityFrameworkCore;
using Northwind.Infrastructure.Repositories;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Infrastructure.UnitTests.Repositories
{
    public class EntityRepositoryTests
    {
        private static int _dbCounter;
        private EntityRepository<Entity> _repository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<EntityDbContext>()
                .UseInMemoryDatabase("TestDB-" + $"{++_dbCounter}")
                .Options;

            var context = new EntityDbContext(options);
            context.AddRange(Data.Entities);
            context.SaveChanges();
            context.ChangeTracker.Clear();

            _repository = new EntityRepository<Entity>(context);
        }

        [Test]
        public async Task InsertAsync_Normally()
        {
            // Act
            await _repository.InsertAsync(new Entity());

            // Assert
            var entities = await _repository.FindAllAsync();

            Assert.AreEqual(Data.Entities.Length + 1, entities.Count());
        }

        [TestCase(1, "UpdatedEntity-1")]
        [TestCase(7, "UpdatedEntity-7")]
        [TestCase(10, "UpdatedEntity-10")]
        public async Task UpdateAsync_Normally(int currentEntityId, string newName)
        {
            // Arrange
            var currentEntity = await _repository.FindAsync(x => x.Id == currentEntityId);
            var newEntity = new Entity() { Id = currentEntity.Id, Name = newName };

            // Act
            await _repository.UpdateAsync(newEntity);

            // Assert
            var updatedEntity = await _repository.FindAsync(x => x.Id == currentEntityId);

            Assert.AreEqual(newName, updatedEntity.Name);
        }

        [TestCase(1)]
        [TestCase(4)]
        [TestCase(10)]
        public async Task DeleteAsync_Normally(int entityIdToDelete)
        {
            // Act
            await _repository.DeleteAsync(new Entity() { Id = entityIdToDelete });

            // Assert
            var res = await _repository.FindAllAsync();
            Assert.AreEqual(Data.Entities.Length - 1, res.Count());
        }

        [TestCase(1)]
        [TestCase(3)]
        [TestCase(10)]
        public async Task FindAsync_FindByIdNormally(int testId)
        {
            // Act
            var result = await _repository.FindAsync(x => x.Id == testId);

            // Assert
            Assert.AreEqual(testId, result.Id);
        }

        [Test]
        public async Task FindAllAsync_Normally()
        {
            // Act
            var result = await _repository.FindAllAsync();

            // Assert
            Assert.AreEqual(Data.Entities.Length, result.Count());
        }

        private static class Data
        {
            public static Entity[] Entities { get; } =
                Enumerable.Range(1, 10).Select(i => new Entity { Id = i, Name = $"Entity-{i}" }).ToArray();
        }

        private class EntityDbContext : DbContext
        {
            public DbSet<Entity> Entities { get; set; }

            public EntityDbContext(DbContextOptions<EntityDbContext> options)
                : base(options) { }
        }

        private class Entity
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }
    }
}
