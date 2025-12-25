using System.Diagnostics.CodeAnalysis;
using GrisAPI.DbContext;
using GrisAPI.DTOs;
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
        _dbContext.Database.EnsureDeleted();
        _dbContext.Database.EnsureCreated();
        
        _sut = new CreatureRepository(_dbContext);
    }
    
    [TearDown]
    public void TearDown()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    [Test]
    public async Task GetFilteredCreatures_validRequest_ReturnsCreatureDtoList()
    {
        //Arrange
        var creatureFilterRequest = new CreatureFilterRequest
        {
            Quantity = 10
        };
        
        var user = new User()
        {
            Id = 0,
            Name = "TestUser"
        };

        var creature = new Creature()
        {
            Id = 1,
            Name = "TestCreature",
            Users = new List<User>()
            {
                user
            }
        };
        
        _dbContext.Creatures.Add(creature);
        await _dbContext.SaveChangesAsync();
        
        //Act
        var results = await _sut.GetFilteredCreatures(creatureFilterRequest, user.Id);
        
        //Assert
        Assert.That(results, Is.Not.Null);
        Assert.That(results.Count, Is.EqualTo(1));
        Assert.That(results.First().Id, Is.EqualTo(creature.Id));
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
    
    [Test]
    public async Task UpdateCreature_ValidCreature_DoesntThrowException()
    {
        //Arrange
        var creature = new Creature()
        {
            Id = 6,
            Name = "Test"
        };
        
        _dbContext.Creatures.Add(creature);
        await _dbContext.SaveChangesAsync();
        var updatedCreature = _dbContext.Creatures.Find(creature.Id);
        updatedCreature.Name = "TestUpdated";
        
        //Act && Assert
        Assert.DoesNotThrowAsync(() => _sut.UpdateCreature(updatedCreature!)); 
    }
    
    [Test]
    public async Task DeleteCreature_ValidCreature_DoesntThrowException()
    {
        //Arrange
        var creature = new Creature()
        {
            Id = 6,
            Name = "Test"
        };
        
        _dbContext.Creatures.Add(creature);
        await _dbContext.SaveChangesAsync();
        var creatureToBeDeleted = _dbContext.Creatures.Find(creature.Id);
        
        //Act && Assert
        Assert.DoesNotThrowAsync(() => _sut.DeleteCreature(creatureToBeDeleted!)); 
    }
}