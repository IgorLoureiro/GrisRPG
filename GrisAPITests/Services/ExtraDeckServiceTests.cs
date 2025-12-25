using System.Diagnostics.CodeAnalysis;
using GrisAPI.DTOs;
using GrisAPI.Models;
using GrisAPI.Repositories.CardRepository;
using GrisAPI.Repositories.ExtraDeckRepository;
using GrisAPI.Repositories.JokerRepository;
using GrisAPI.Services.ExtraDeckService;
using MockQueryable;
using Moq;

namespace GrisAPITests.Services;

[TestFixture]
[ExcludeFromCodeCoverage]
public sealed class ExtraDeckServiceTests
{
    private ExtraDeckService _sut;
    private Mock<IExtraDeckRepository> _extraDeckRepositoryMock;
    private Mock<ICardRepository> _cardRepositoryMock;
    private Mock<IJokerRepository> _jokerRepositoryMock;

    [SetUp]
    public void SetUp()
    {
        _extraDeckRepositoryMock = new Mock<IExtraDeckRepository>();
        _cardRepositoryMock = new Mock<ICardRepository>();
        _jokerRepositoryMock = new Mock<IJokerRepository>();
        
        _sut = new ExtraDeckService(
            _extraDeckRepositoryMock.Object,
            _cardRepositoryMock.Object,
            _jokerRepositoryMock.Object);
    }

    [Test]
    public async Task GetExtraDeckById_ValidId_ReturnsExtraDeck()
    {
        //Arrange
        var extraDeckId = 1;
        var extraDeckModel = new ExtraDeck
        {
            Id = extraDeckId,
            CreatureId = 1,
            Creature = new Creature
            {
                Id = 1,
                Name = "Creature"
            },
        };
        
        _extraDeckRepositoryMock.Setup(x => x.GetExtraDeckById(extraDeckId)).ReturnsAsync(extraDeckModel);
        
        //Act
        var result = await _sut.GetExtraDeckById(extraDeckId);
        
        //Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<ExtraDeckDto>());
        Assert.That(result.Id, Is.EqualTo(extraDeckId));
    }

    [Test]
    public async Task UpdateExtraDeck_ValidExtraDeckInput_ReturnsTrue()
    {
        //Arrange
        const int extraDeckId = 1;
        
        var extraDeckModel = new ExtraDeck
        {
            Id = extraDeckId,
            CreatureId = 1,
            Creature = new Creature
            {
                Id = 1,
                Name = "Creature"
            },
        };

        var cardsList = new List<Card>
        {
            new Card
            {
                Id = 1,
                Name = "Card 1",
            },
            new Card
            {
                Id = 2,
                Name = "Card 2",
            }
        };

        var jokersList = new List<Joker>
        {
            new Joker
            {
                Id = 0,
                Name = "Joker"
            },
            new Joker
            {
                Id = 1,
                Name = "Joker"
            }
        };
        
        _extraDeckRepositoryMock.Setup(x => x.GetExtraDeckById(extraDeckId)).ReturnsAsync(extraDeckModel);
        _cardRepositoryMock.Setup(x => x.GetCardsById(It.IsAny<IEnumerable<int>>())).Returns(cardsList.BuildMock());
        _jokerRepositoryMock.Setup(x => x.GetJokersById(It.IsAny<IEnumerable<int>>())).ReturnsAsync(jokersList);
        
        //Act
        var result = await _sut.UpdateExtraDeck(new ExtraDeckDto(extraDeckModel));
        
        //Assert
        Assert.That(result, Is.True);
    }
}