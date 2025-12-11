using System.Diagnostics.CodeAnalysis;
using GrisAPI.DbContext;
using GrisAPI.Models;
using GrisAPI.Repositories.ExtraDeckRepository;
using Microsoft.EntityFrameworkCore;

namespace GrisAPITests.Repositories;

[TestFixture]
[ExcludeFromCodeCoverage]
public sealed class ExtraDeckRepositoryTests
{
    private ExtraDeckRepository _sut = null!;
    private ApplicationDbContext _dbContext = null!;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "ExtraDeckRepositoryTests")
            .Options;
        
        _dbContext = new ApplicationDbContext(options);
        _dbContext.Database.EnsureDeleted();
        _dbContext.Database.EnsureCreated();
        
        _sut = new ExtraDeckRepository(_dbContext);
    }

    [Test]
    public async Task GetExtraDeckById_ValidId_ReturnsExtraDeck()
    {
        //Arrange
        var extraDeckModel = new ExtraDeck
        {
            Id = 1,
            CreatureId = 1,
            Creature = new Creature { Id = 1 }
        };
        
        _dbContext.ExtraDecks.Add(extraDeckModel);
        await _dbContext.SaveChangesAsync();
        
        //Act
        var result = await _sut.GetExtraDeckById(1);
        
        //Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(1));
    }
    
    [Test]
    public async Task UpdateExtraDeck_ValidExtraDeck_DoesntThrowException()
    {
        //Arrange
        var extraDeckModel = new ExtraDeck
        {
            Id = 2,
            CreatureId = 2,
            Creature = new Creature
            {
                Id = 2,
                Name = "Name"
            }
        };
        
        _dbContext.ExtraDecks.Add(extraDeckModel);
        await _dbContext.SaveChangesAsync();
        
        extraDeckModel.Creature.Name = "UpdatedName";
        
        //Act & Assert
        Assert.DoesNotThrowAsync(() => _sut.UpdateExtraDeck(extraDeckModel));
    }
}