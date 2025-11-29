using System.Diagnostics.CodeAnalysis;
using GrisAPI.DbContext;
using GrisAPI.Models;
using GrisAPI.Repositories.UserRepository;
using Microsoft.EntityFrameworkCore;

namespace GrisAPITests.Repositories
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class UserRepositoryTests
    {
        private ApplicationDbContext _dbContext = null!;
        private UserRepository _repository = null!;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "UserRepositoryTests_DB")
                .Options;

            _dbContext = new ApplicationDbContext(options);
            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();

            _repository = new UserRepository(_dbContext);
        }

        [Test]
        public async Task GetUserByUsernameAsync_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var user = new User
            {
                Name = "testuser",
                PasswordHash = "hash",
                Attempts = 0,
                IsBlocked = false
            };

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _repository.GetUserByUsernameAsync("testuser");

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Name, Is.EqualTo("testuser"));
        }

        [Test]
        public async Task GetUserByUsernameAsync_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Act
            var result = await _repository.GetUserByUsernameAsync("ghost");

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task UpdateUserAsync_ShouldPersistUpdatedValues()
        {
            // Arrange
            var user = new User
            {
                Name = "target",
                PasswordHash = "hash",
                Attempts = 1,
                IsBlocked = false
            };

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
            
            user.Attempts = 4;
            user.IsBlocked = true;

            // Act
            await _repository.UpdateUserAsync(user);

            // Assert
            var updated = await _dbContext.Users.FirstAsync(u => u.Name == "target");

            Assert.That(updated.Attempts, Is.EqualTo(4));
            Assert.That(updated.IsBlocked, Is.True);
        }
    }
}
