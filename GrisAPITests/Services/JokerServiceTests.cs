using GrisAPI.DTOs;
using GrisAPI.Models;
using GrisAPI.Repositories.JokerRepository;
using GrisAPI.Services.JokerService;
using Moq;

namespace GrisAPITests.Services;

public class JokerServiceTests
{
    private JokerService _sut = null!;
    private Mock<IJokerRepository> _jokerRepository = null!;
    
    [SetUp]
    public void Setup()
    {
        _jokerRepository = new Mock<IJokerRepository>();
        _sut = new JokerService(_jokerRepository.Object);
    }
    
    [Test]
    public async Task GetJokerById_ExistsJokerWithId_ReturnsJoker()
    {
        // Arrange
        var joker = new Joker
        {
            Id = 1,
            Name = "Joker",
            Description = "Joker Description"
        };
        _jokerRepository.Setup(x => x.GetJokerById(1)).ReturnsAsync(joker);
        
        // Act
        var result = await _sut.GetJokerById(1);
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(1));
        Assert.That(result.Name, Is.EqualTo("Joker"));
    }
    
    [Test]
    public async Task GetJokerById_JokerDoesNotExist_ReturnsNull()
    {
        // Arrange
        _jokerRepository.Setup(x => x.GetJokerById(It.IsAny<int>())).ReturnsAsync((Joker?)null);
        
        // Act
        var result = await _sut.GetJokerById(999);
        
        // Assert
        Assert.That(result, Is.Null);
    }
    
    [Test]
    public async Task GetJokersById_WithListOfIds_ReturnsJokerList()
    {
        // Arrange
        var jokers = new List<Joker>
        {
            new Joker { Id = 1, Name = "Joker1", Description = "Desc1" },
            new Joker { Id = 2, Name = "Joker2", Description = "Desc2" }
        };
        var ids = new List<int> { 1, 2 };
        _jokerRepository.Setup(x => x.GetJokersById(ids)).ReturnsAsync(jokers);
        
        // Act
        var result = await _sut.GetJokersById(ids);
        
        // Assert
        Assert.That(result, Has.Count.EqualTo(2));
        Assert.That(result[0].Id, Is.EqualTo(1));
        Assert.That(result[1].Id, Is.EqualTo(2));
    }
    
    [Test]
    public async Task GetJokersByName_WithName_ReturnsJokerList()
    {
        // Arrange
        var jokers = new List<Joker>
        {
            new Joker { Id = 1, Name = "Joker", Description = "Desc1" },
            new Joker { Id = 2, Name = "Joker", Description = "Desc2" }
        };
        _jokerRepository.Setup(x => x.GetJokersByName("Joker")).ReturnsAsync(jokers);
        
        // Act
        var result = await _sut.GetJokersByName("Joker");
        
        // Assert
        Assert.That(result, Has.Count.EqualTo(2));
        Assert.That(result[0].Name, Is.EqualTo("Joker"));
    }
    
    [Test]
    public async Task AddJoker_WithValidDto_ReturnsAddedJoker()
    {
        // Arrange
        var jokerDto = new JokerDto
        {
            Name = "New Joker",
            Description = "New Description"
        };
        var addedJoker = new Joker
        {
            Id = 1,
            Name = "New Joker",
            Description = "New Description"
        };
        _jokerRepository.Setup(x => x.AddJoker(It.IsAny<Joker>())).ReturnsAsync(addedJoker);
        
        // Act
        var result = await _sut.AddJoker(jokerDto);
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Name, Is.EqualTo("New Joker"));
        Assert.That(result.Description, Is.EqualTo("New Description"));
        _jokerRepository.Verify(x => x.AddJoker(It.Is<Joker>(j => 
            j.Name == "New Joker" && j.Description == "New Description")), Times.Once);
    }
    
    [Test]
    public async Task AddJoker_WithNullDto_ReturnsNull()
    {
        // Act
        var result = await _sut.AddJoker(null);
        
        // Assert
        Assert.That(result, Is.Null);
        _jokerRepository.Verify(x => x.AddJoker(It.IsAny<Joker>()), Times.Never);
    }
    
    [Test]
    public async Task UpdateJoker_WithValidDto_ReturnsTrue()
    {
        // Arrange
        var existingJoker = new Joker
        {
            Id = 1,
            Name = "Old Name",
            Description = "Old Description"
        };
        var jokerDto = new JokerDto
        {
            Id = 1,
            Name = "Updated Name",
            Description = "Updated Description"
        };
        _jokerRepository.Setup(x => x.GetJokerById(1)).ReturnsAsync(existingJoker);
        _jokerRepository.Setup(x => x.UpdateJoker(It.IsAny<Joker>())).Returns(Task.CompletedTask);
        
        // Act
        var result = await _sut.UpdateJoker(jokerDto);
        
        // Assert
        Assert.That(result, Is.True);
        Assert.That(existingJoker.Name, Is.EqualTo("Updated Name"));
        Assert.That(existingJoker.Description, Is.EqualTo("Updated Description"));
        _jokerRepository.Verify(x => x.UpdateJoker(existingJoker), Times.Once);
    }
    
    [Test]
    public async Task UpdateJoker_WithNullDto_ReturnsFalse()
    {
        // Act
        var result = await _sut.UpdateJoker(null);
        
        // Assert
        Assert.That(result, Is.False);
        _jokerRepository.Verify(x => x.UpdateJoker(It.IsAny<Joker>()), Times.Never);
    }
    
    [Test]
    public async Task UpdateJoker_JokerDoesNotExist_ReturnsFalse()
    {
        // Arrange
        var jokerDto = new JokerDto
        {
            Id = 999,
            Name = "Updated Name",
            Description = "Updated Description"
        };
        _jokerRepository.Setup(x => x.GetJokerById(999)).ReturnsAsync((Joker?)null);
        
        // Act
        var result = await _sut.UpdateJoker(jokerDto);
        
        // Assert
        Assert.That(result, Is.False);
        _jokerRepository.Verify(x => x.UpdateJoker(It.IsAny<Joker>()), Times.Never);
    }
    
    [Test]
    public async Task DeleteJoker_JokerExists_ReturnsTrue()
    {
        // Arrange
        var existingJoker = new Joker
        {
            Id = 1,
            Name = "Joker",
            Description = "Description"
        };
        _jokerRepository.Setup(x => x.GetJokerById(1)).ReturnsAsync(existingJoker);
        _jokerRepository.Setup(x => x.DeleteJoker(existingJoker)).Returns(Task.CompletedTask);
        
        // Act
        var result = await _sut.DeleteJoker(1);
        
        // Assert
        Assert.That(result, Is.True);
        _jokerRepository.Verify(x => x.DeleteJoker(existingJoker), Times.Once);
    }
    
    [Test]
    public async Task DeleteJoker_JokerDoesNotExist_ReturnsFalse()
    {
        // Arrange
        _jokerRepository.Setup(x => x.GetJokerById(999)).ReturnsAsync((Joker?)null);
        
        // Act
        var result = await _sut.DeleteJoker(999);
        
        // Assert
        Assert.That(result, Is.False);
        _jokerRepository.Verify(x => x.DeleteJoker(It.IsAny<Joker>()), Times.Never);
    }
}