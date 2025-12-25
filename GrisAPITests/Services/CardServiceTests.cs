using GrisAPI.DTOs;
using GrisAPI.Models;
using GrisAPI.Models.Enums;
using GrisAPI.Repositories.CardRepository;
using GrisAPI.Services.CardService;
using MockQueryable;
using Moq;

namespace GrisAPITests.Services;

public class CardServiceTests
{
    private CardService _sut = null!;
    private Mock<ICardRepository> _cardRepositoryMock = null!;
    
    [SetUp]
    public void Setup()
    {
        _cardRepositoryMock = new Mock<ICardRepository>();
        _sut = new CardService(_cardRepositoryMock.Object);
    }

    [Test]
    public async Task GetFilteredCards_validFilter_ReturnsCardFilterResponse()
    {
        //Arrange
        var cardsList = new List<Card>
        {
            new Card
            {
                Id = 0,
                Name = "TestCard",
                Description = "TestCard",
                Manifestation = Manifestation.Artillery,
                Symbol = Symbol.Clubs
            }
        };

        var filter = new CardFilterRequest
        {
            CurrentPage = 10
        };
        
        _cardRepositoryMock.Setup(x => 
                x.GetFilteredCards(It.IsAny<CardFilterRequest>()))
                .Returns(cardsList.BuildMock());
        
        //Act
        var result = await _sut.GetFilteredCards(filter);
        
        //Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Cards.Count, Is.GreaterThan(0));
        Assert.That(result.MaxNumberOfPages, Is.GreaterThan(0));
    }

    [Test]
    public async Task GetCardsById_ValidIdList_ReturnsCardDtoList()
    {
        //Arrange
        var cardList = new List<Card>
        {
            new Card
            {
                Id = 0,
                Name = "TestCard",
                Description = "TestCard",
            }
        };

        var idList = new List<int>
        {
            0
        };
        
        _cardRepositoryMock.Setup(x => 
                x.GetCardsById(It.IsAny<List<int>>()))
            .Returns(cardList.BuildMock());
        
        //Act
        var result = await _sut.GetCardsById(idList);
        
        //Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.GreaterThan(0));
        Assert.That(result.First().Id, Is.EqualTo(0));
    }
    
    [Test]
    public async Task GetCardById_ValidId_ReturnsCard()
    {
        //Arrange
        var card = new Card
        {
            Id = 0,
            Name = "TestCard",
            Description = "TestCard",
        };

        _cardRepositoryMock.Setup(x => 
            x.GetCardById(0))
            .ReturnsAsync(card);
        
        //Act
        var result = await _sut.GetCardById(0);
        
        //Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.EqualTo(card));
    }
    
    [Test]
    public async Task AddCard_ValidCardDto_ReturnsCard()
    {
        //Arrange
        var card = new CardDto
        {
            Id = 0,
            Name = "TestCard",
            Description = "TestCard",
        };
        
        var cardModel = new Card
        {
            Id = 0,
            Name = "TestCard",
            Description = "TestCard",
        };

        _cardRepositoryMock.Setup(x => x.AddCard(It.IsAny<Card>())).ReturnsAsync(cardModel);
        
        //Act
        var result = await _sut.AddCard(card);
        
        //Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.EqualTo(cardModel));
    }

    [Test]
    public async Task UpdateCard_ValidCardDto_ReturnsTrue()
    {
        // Arrange
        var cardDto = new CardDto
        {
            Id = 1,
            Name = "Updated Name",
            Description = "Updated Description",
            Symbol = Symbol.Clubs,
            Manifestation = Manifestation.Artillery
        };

        var existingCard = new Card
        {
            Id = 1,
            Name = "Old Name",
            Description = "Old Description",
            Symbol = Symbol.Hearts,
            Manifestation = Manifestation.Artillery
        };

        _cardRepositoryMock
            .Setup(x => x.GetCardById(cardDto.Id))
            .ReturnsAsync(existingCard);

        _cardRepositoryMock
            .Setup(x => x.UpdateCard(It.IsAny<Card>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _sut.UpdateCard(cardDto);

        // Assert
        Assert.That(result, Is.True);

        Assert.That(existingCard.Name, Is.EqualTo(cardDto.Name));
        Assert.That(existingCard.Description, Is.EqualTo(cardDto.Description));
        Assert.That(existingCard.Symbol, Is.EqualTo(cardDto.Symbol));
        Assert.That(existingCard.Manifestation, Is.EqualTo(cardDto.Manifestation));

        _cardRepositoryMock.Verify(
            x => x.UpdateCard(existingCard),
            Times.Once);
    }

    [Test]
    public async Task DeleteCard_ValidId_ReturnsTrue()
    {
        // Arrange
        var cardId = 1;

        var existingCard = new Card
        {
            Id = cardId,
            Name = "TestCard",
            Description = "TestCard"
        };

        _cardRepositoryMock
            .Setup(x => x.GetCardById(cardId))
            .ReturnsAsync(existingCard);

        _cardRepositoryMock
            .Setup(x => x.DeleteCard(It.IsAny<Card>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _sut.DeleteCard(cardId);

        // Assert
        Assert.That(result, Is.True);

        _cardRepositoryMock.Verify(
            x => x.DeleteCard(existingCard),
            Times.Once);
    }
}