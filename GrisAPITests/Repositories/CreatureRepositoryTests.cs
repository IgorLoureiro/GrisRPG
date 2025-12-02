using System.Diagnostics.CodeAnalysis;
using GrisAPI.DbContext;
using GrisAPI.Models;
using GrisAPI.Repositories.CreatureRepository;
using Microsoft.EntityFrameworkCore;

namespace GrisAPITests.Repositories;

[TestFixture]
[ExcludeFromCodeCoverage]
public class CreatureRepositoryTests
{
    private ApplicationDbContext _dbContext = null!;
    private CreatureRepository _sut = null!;

    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "UserRepositoryTests_DB")
            .Options;

        _dbContext = new ApplicationDbContext(options);
        _dbContext.Database.EnsureCreated();
        _dbContext.Database.EnsureDeleted();
        
        _sut = new CreatureRepository(_dbContext);
    }

    [Test]
    public async Task CreateAsync_ValidCreatureModel_CreatesSuccessfully()
    {
        //Arrange
        var creature = new Creature()
        {
            Name = "Test",
        };
        
        _dbContext.Creatures.Add(creature);
        
        //Act
        var result = await _sut.CreateAsync(creature);

        //Assert
        Assert.That(result, !Is.Null);
        Assert.That(result.Name, Is.EqualTo(creature.Name));
    }
    
    [Test]
    public async Task GetCreatureByIdAsync_ValidId_ReturnsSuccessfully()
    {
        //Arrange
        var creature = new Creature()
        {
            Id = 5,
            Name = "Test",
        };
        
        _dbContext.Creatures.Add(creature);
        
        //Act
        var result = await _sut.GetCreatureByIdAsync(creature.Id);

        //Assert
        Assert.That(result, !Is.Null);
        Assert.That(result.Name, Is.EqualTo(creature.Name));
    }
}