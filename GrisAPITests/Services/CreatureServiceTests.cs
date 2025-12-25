using System.Diagnostics.CodeAnalysis;
using GrisAPI.DTOs;
using GrisAPI.Models;
using GrisAPI.Repositories.CreatureRepository;
using GrisAPI.Repositories.UserRepository;
using GrisAPI.Services.CreatureService;
using Moq;

namespace GrisAPITests.Services;

[TestFixture]
[ExcludeFromCodeCoverage]
public class CreatureServiceTests
{
    private CreatureService _sut;
    private Mock<IUserRepository> _mockUserRepository;
    private Mock<ICreatureRepository> _mockCreatureRepository;

    [SetUp]
    public void SetUp()
    {
        _mockCreatureRepository = new Mock<ICreatureRepository>();
        _mockUserRepository = new Mock<IUserRepository>();
        _sut = new CreatureService(_mockUserRepository.Object, _mockCreatureRepository.Object);
    }

    [Test]
    public async Task GetFilteredCreatures_validFilterAndUserId_ReturnsCreatureFilterResponseWithFilledList()
    {
        //Arrange
        var creatureDtoList = new List<CreatureDto>
        {
            new CreatureDto
            {
                Id = 0,
                Name = "TestCreature"
            }
        };

        var filter = new CreatureFilterRequest
        {
            Name = null!,
            CurrentPage = 0,
            Quantity = 10
        };
        
        _mockCreatureRepository.Setup(x => 
            x.GetFilteredCreatures(It.IsAny<CreatureFilterRequest>(), It.IsAny<int>()))
            .ReturnsAsync(creatureDtoList);
        
        //Act
        var result = await _sut.GetFilteredCreatures(filter, 0);
        
        //Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Creatures, Has.Count.EqualTo(creatureDtoList.Count));
        Assert.That(result.MaxNumberOfPages, Is.GreaterThan(0));
    }

    [Test]
    public async Task CreateCreature_validInput_CompletesSucessfully()
    {
        //Arrange
        var user = new User
        {
            Id = 0,
            Name = "John Doe",
        };
        
        var creature = new Creature
        {
            Name = "Test Creature",
            Id = 0
        };
        
        _mockCreatureRepository.Setup(x => x.CreateAsync(It.IsAny<Creature>())).ReturnsAsync(creature);
        _mockUserRepository.Setup(x => x.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync(user);

        //Act
        var result = await _sut.CreateCreature(creature.Name, user.Id);
        
        //Assert
        Assert.That(result, !Is.Null);
        Assert.That(result.Id, Is.EqualTo(creature.Id));
    }
    
    [Test]
    public async Task GetCreatureById_ExistentId_ReturnCreatureDto()
    {
        //Arrange
        var creature = new Creature
        {
            Id = 11,
            Name = "Test Creature",
        };
        
        _mockCreatureRepository.Setup(x => x.GetCreatureByIdAsync(It.IsAny<Int32>())).ReturnsAsync(creature);
        
        //Act
        var result = await _sut.GetCreatureById(creature.Id);
        
        //Assert
        Assert.That(result, !Is.Null);
        Assert.That(result, Is.TypeOf<CreatureDto>());
        Assert.That(result.Id, Is.EqualTo(creature.Id));
    }
    
    [Test]
    public async Task UpdateCreature_ExistingCreature_SuccessfullyUpdates()
    {
        //Arrange
        var creature = new Creature
        {
            Id = 11,
            Name = "Test Creature",
        };

        var updatedCreature = new CreatureDto
        {
            Id = 11,
            Name = "Updated Creature",
        };
        
        _mockCreatureRepository.Setup(x => x.GetCreatureByIdAsync(It.IsAny<Int32>())).ReturnsAsync(creature);
        
        //Act
        var result = await _sut.UpdateCreature(updatedCreature);
        
        //Assert
        Assert.That(result, !Is.Null);
        Assert.That(result, Is.True);
    }
    
    [Test]
    public async Task DeleteCreature_ExistingCreature_SuccessfullyDeletes()
    {
        //Arrange
        var creature = new Creature
        {
            Id = 11,
            Name = "Test Creature",
        };
        
        _mockCreatureRepository.Setup(x => x.GetCreatureByIdAsync(It.IsAny<Int32>())).ReturnsAsync(creature);
        
        //Act
        var result = await _sut.DeleteCreatureById(creature.Id);
        
        //Assert
        Assert.That(result, !Is.Null);
        Assert.That(result, Is.True);
    }
}