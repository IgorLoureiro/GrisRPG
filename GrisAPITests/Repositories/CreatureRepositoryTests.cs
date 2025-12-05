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
    
    [Test]
    public async Task GetAllCreaturesFromUser_ValidUserId_ReturnsCreatureDtoList()
    {
        //Arrange
        const int userId = 1;
        
        var creature = new Creature()
        {
            Id = 5,
            Name = "Test",
            Users = new List<User>()
            {
                new User()
                {
                    Id = userId,
                    Name = "TestUser",
                }
            }
        };
        
        _dbContext.Creatures.Add(creature);
        
        //Act
        var result = await _sut.GetAllCreaturesFromUser(userId);

        //Assert
        Assert.That(result, !Is.Null);
        Assert.That(result, Is.TypeOf<List<CreatureDto>>());
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