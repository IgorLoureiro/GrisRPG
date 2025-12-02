using System.Diagnostics.CodeAnalysis;
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
}