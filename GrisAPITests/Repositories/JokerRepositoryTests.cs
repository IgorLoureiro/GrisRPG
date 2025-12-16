using GrisAPI.DbContext;
using GrisAPI.Models;
using GrisAPI.Repositories.JokerRepository;
using Microsoft.EntityFrameworkCore;

namespace GrisAPITests.Repositories;

public class JokerRepositoryTests
{
    private JokerRepository _sut = null!;
    private ApplicationDbContext _dbContext = null!;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "JokerRepositoryTests")
            .Options;
        
        _dbContext = new ApplicationDbContext(options);
        _dbContext.Database.EnsureDeleted();
        _dbContext.Database.EnsureCreated();
        
        _sut = new JokerRepository(_dbContext);
    }
    
    [TearDown]
    public void TearDown()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    [Test]
    public async Task GetJokerById_JokerExists_ReturnsJokerModel()
    {
        //Arrange
        const int jokerId = 1;

        var jokerModel = new Joker()
        {
            Id = jokerId,
            Name = "Joker",
            Description = "Joker"
        };
        
        _dbContext.Jokers.Add(jokerModel);
        await _dbContext.SaveChangesAsync();
        
        //Act
        var results = await _sut.GetJokerById(jokerId);
        
        //Assert
        Assert.That(results, Is.Not.Null);
        Assert.That(results, Is.TypeOf<Joker>());
        Assert.That(results, Is.EqualTo(jokerModel));
    }
    
    [Test]
    public async Task GetJokersByIds_JokersExists_ReturnsJokerList()
    {
        //Arrange
        const int jokerId = 1;

        var jokerModel = new Joker()
        {
            Id = jokerId,
            Name = "Joker",
            Description = "Joker"
        };
        
        _dbContext.Jokers.Add(jokerModel);
        await _dbContext.SaveChangesAsync();

        var listIds = new List<int>
        {
            jokerId,
        };
        
        //Act
        var results = await _sut.GetJokersById(listIds);
        
        //Assert
        Assert.That(results, Is.Not.Null);
        Assert.That(results.First(), Is.EqualTo(jokerModel));
    }
    
    [Test]
    public async Task GetJokersByName_JokerExist_ReturnsJokerList()
    {
        //Arrange
        const int jokerId = 1;

        var jokerModel = new Joker()
        {
            Id = jokerId,
            Name = "Joker",
            Description = "Joker"
        };
        
        _dbContext.Jokers.Add(jokerModel);
        await _dbContext.SaveChangesAsync();
        
        //Act
        var results = await _sut.GetJokersByName(jokerModel.Name);
        
        //Assert
        Assert.That(results, Is.Not.Null);
        Assert.That(results.First(), Is.EqualTo(jokerModel));
    }

    [Test]
    public async Task AddJoker_ValidJokerObject_returnJoker()
    {
        //Arrange
        var jokerModel = new Joker()
        {
            Id = 1,
            Name = "Joker",
            Description = "Joker"
        };
        
        //Act
        var result = await _sut.AddJoker(jokerModel);
        
        //Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(jokerModel.Id));
    }
    
    [Test]
    public async Task UpdateJoker_ValidJokerObject_DoNotThrowException()
    {
        //Arrange
        var jokerModel = new Joker()
        {
            Id = 1,
            Name = "Joker",
            Description = "Joker"
        };
        
        _dbContext.Jokers.Add(jokerModel);
        await _dbContext.SaveChangesAsync();
        
        jokerModel.Name = "UpdatedJoker";
        
        //Act & Assert
        Assert.DoesNotThrowAsync(() => _sut.UpdateJoker(jokerModel));
    }
    
    [Test]
    public async Task DeleteJoker_ValidJokerObject_DoNotThrowException()
    {
        //Arrange
        var jokerModel = new Joker()
        {
            Id = 1,
            Name = "Joker",
            Description = "Joker"
        };
        
        _dbContext.Jokers.Add(jokerModel);
        await _dbContext.SaveChangesAsync();
        
        //Act & Assert
        Assert.DoesNotThrowAsync(() => _sut.DeleteJoker(jokerModel));
    }
}