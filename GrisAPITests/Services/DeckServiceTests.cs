using System.Diagnostics.CodeAnalysis;
using GrisAPI.DTOs;
using GrisAPI.Models;
using GrisAPI.Repositories.CardRepository;
using GrisAPI.Repositories.CreatureRepository;
using GrisAPI.Repositories.DeckRepository;
using GrisAPI.Repositories.JokerRepository;
using GrisAPI.Services.DeckService;
using MockQueryable;
using Moq;

namespace GrisAPITests.Services;

[TestFixture]
[ExcludeFromCodeCoverage]
public sealed class DeckServiceTests
{
    private DeckService _sut;
    private Mock<IDeckRepository> _deckRepositoryMock;
    private Mock<ICardRepository> _cardRepositoryMock;
    private Mock<IJokerRepository> _jokerRepositoryMock;
    private Mock<ICreatureRepository> _creatureRepositoryMock;

    [SetUp]
    public void SetUp()
    {
        _deckRepositoryMock = new Mock<IDeckRepository>();
        _cardRepositoryMock = new Mock<ICardRepository>();
        _jokerRepositoryMock = new Mock<IJokerRepository>();
        _creatureRepositoryMock = new Mock<ICreatureRepository>();
        
        _sut = new DeckService(
            _deckRepositoryMock.Object,
            _cardRepositoryMock.Object,
            _jokerRepositoryMock.Object,
            _creatureRepositoryMock.Object);
    }

    #region GetDeckById Tests

    [Test]
    public async Task GetDeckById_ValidId_ReturnsDeckDto()
    {
        // Arrange
        var deckId = 1;
        var deckModel = new Deck
        {
            Id = deckId,
            Name = "Test Deck",
            Creatures = new List<Creature>
            {
                new Creature { Id = 1, Name = "Test Creature" }
            },
            Cards = new List<Card>(),
            Jokers = new List<Joker>()
        };
        
        _deckRepositoryMock.Setup(x => x.GetDeckById(deckId)).ReturnsAsync(deckModel);
        
        // Act
        var result = await _sut.GetDeckById(deckId);
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<DeckDto>());
        Assert.That(result.Id, Is.EqualTo(deckId));
        Assert.That(result.Name, Is.EqualTo("Test Deck"));
    }

    [Test]
    public async Task GetDeckById_InvalidId_ReturnsNull()
    {
        // Arrange
        var deckId = 999;
        _deckRepositoryMock.Setup(x => x.GetDeckById(deckId)).ReturnsAsync((Deck?)null);
        
        // Act
        var result = await _sut.GetDeckById(deckId);
        
        // Assert
        Assert.That(result, Is.Null);
    }

    #endregion

    #region AddDeck Tests

    [Test]
    public async Task AddDeck_ValidCreatureId_ReturnsDeckDto()
    {
        // Arrange
        var creatureId = 1;
        var creature = new Creature
        {
            Id = creatureId,
            Name = "Test Creature"
        };

        var deckDto = new DeckDto
        {
            Name = "New Deck",
            Cards = new List<CardDto>(),
            Jokers = new List<JokerDto>()
        };

        var savedDeck = new Deck
        {
            Id = 5,
            Name = "New Deck",
            Creatures = new List<Creature> { creature }
        };

        _creatureRepositoryMock
            .Setup(x => x.GetCreatureByIdAsync(creatureId))
            .ReturnsAsync(creature);

        _deckRepositoryMock
            .Setup(x => x.AddDeck(It.Is<Deck>(d => 
                d.Name == deckDto.Name && 
                d.Creatures.First().Id == creatureId)))
            .ReturnsAsync(savedDeck);
        
        // Act
        var result = await _sut.AddDeck(deckDto, creatureId);
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(5));
        Assert.That(result.Name, Is.EqualTo("New Deck"));
        _creatureRepositoryMock.Verify(x => x.GetCreatureByIdAsync(creatureId), Times.Once);
        _deckRepositoryMock.Verify(x => x.AddDeck(It.IsAny<Deck>()), Times.Once);
    }

    [Test]
    public async Task AddDeck_InvalidCreatureId_ReturnsNull()
    {
        // Arrange
        var creatureId = 999;
        var deckDto = new DeckDto
        {
            Name = "New Deck",
            Cards = new List<CardDto>(),
            Jokers = new List<JokerDto>()
        };

        _creatureRepositoryMock
            .Setup(x => x.GetCreatureByIdAsync(creatureId))
            .ReturnsAsync((Creature?)null);
        
        // Act
        var result = await _sut.AddDeck(deckDto, creatureId);
        
        // Assert
        Assert.That(result, Is.Null);
        _creatureRepositoryMock.Verify(x => x.GetCreatureByIdAsync(creatureId), Times.Once);
        _deckRepositoryMock.Verify(x => x.AddDeck(It.IsAny<Deck>()), Times.Never);
    }

    #endregion

    #region UpdateDeck Tests

    [Test]
    public async Task UpdateDeck_ValidDeckWithCardsAndJokers_ReturnsTrue()
    {
        // Arrange
        var deckId = 1;
        var deckModel = new Deck
        {
            Id = deckId,
            Name = "Original Deck",
            Cards = new List<Card>(),
            Jokers = new List<Joker>()
        };

        var cardIds = new List<int> { 1, 2, 3 };
        var jokerIds = new List<int> { 4, 5 };
        
        var cardsList = new List<Card>
        {
            new Card { Id = 1, Name = "Card 1" },
            new Card { Id = 2, Name = "Card 2" },
            new Card { Id = 3, Name = "Card 3" }
        };

        var jokersList = new List<Joker>
        {
            new Joker { Id = 4, Name = "Joker 4" },
            new Joker { Id = 5, Name = "Joker 5" }
        };

        var deckDto = new DeckDto(deckModel)
        {
            Cards = cardsList.Select(c => new CardDto(c)).ToList(),
            Jokers = jokersList.Select(j => new JokerDto(j)).ToList()
        };

        _deckRepositoryMock
            .Setup(x => x.GetDeckById(deckId))
            .ReturnsAsync(deckModel);

        _cardRepositoryMock
            .Setup(x => x.GetCardsById(cardIds))
            .Returns(cardsList.BuildMock());

        _jokerRepositoryMock
            .Setup(x => x.GetJokersById(jokerIds))
            .ReturnsAsync(jokersList);

        _deckRepositoryMock
            .Setup(x => x.UpdateDeck(deckModel))
            .Returns(Task.CompletedTask);
        
        // Act
        var result = await _sut.UpdateDeck(deckDto);
        
        // Assert
        Assert.That(result, Is.True);
        Assert.That(deckModel.Cards.Count, Is.EqualTo(3));
        Assert.That(deckModel.Jokers.Count, Is.EqualTo(2));
        _deckRepositoryMock.Verify(x => x.GetDeckById(deckId), Times.Once);
        _cardRepositoryMock.Verify(x => x.GetCardsById(cardIds), Times.Once);
        _jokerRepositoryMock.Verify(x => x.GetJokersById(jokerIds), Times.Once);
        _deckRepositoryMock.Verify(x => x.UpdateDeck(deckModel), Times.Once);
    }

    [Test]
    public async Task UpdateDeck_ValidDeckEmptyCardsAndJokers_ReturnsTrue()
    {
        // Arrange
        var deckId = 1;
        var deckModel = new Deck
        {
            Id = deckId,
            Name = "Original Deck",
            Cards = new List<Card>
            {
                new Card { Id = 1, Name = "Old Card" }
            },
            Jokers = new List<Joker>
            {
                new Joker { Id = 2, Name = "Old Joker" }
            }
        };

        var deckDto = new DeckDto(deckModel)
        {
            Cards = new List<CardDto>(),
            Jokers = new List<JokerDto>()
        };

        _deckRepositoryMock
            .Setup(x => x.GetDeckById(deckId))
            .ReturnsAsync(deckModel);

        _cardRepositoryMock
            .Setup(x => x.GetCardsById(It.IsAny<IEnumerable<int>>()))
            .Returns(new List<Card>().BuildMock());

        _jokerRepositoryMock
            .Setup(x => x.GetJokersById(It.IsAny<IEnumerable<int>>()))
            .ReturnsAsync(new List<Joker>());

        _deckRepositoryMock
            .Setup(x => x.UpdateDeck(deckModel))
            .Returns(Task.CompletedTask);
        
        // Act
        var result = await _sut.UpdateDeck(deckDto);
        
        // Assert
        Assert.That(result, Is.True);
        Assert.That(deckModel.Cards, Is.Empty);
        Assert.That(deckModel.Jokers, Is.Empty);
        _cardRepositoryMock.Verify(x => x.GetCardsById(It.IsAny<IEnumerable<int>>()), Times.Once);
        _jokerRepositoryMock.Verify(x => x.GetJokersById(It.IsAny<IEnumerable<int>>()), Times.Once);
    }

    [Test]
    public async Task UpdateDeck_DeckNotFound_ReturnsFalse()
    {
        // Arrange
        var deckId = 999;
        var deckDto = new DeckDto
        {
            Id = deckId,
            Name = "Non-existent Deck",
            Cards = new List<CardDto>(),
            Jokers = new List<JokerDto>()
        };

        _deckRepositoryMock
            .Setup(x => x.GetDeckById(deckId))
            .ReturnsAsync((Deck?)null);
        
        // Act
        var result = await _sut.UpdateDeck(deckDto);
        
        // Assert
        Assert.That(result, Is.False);
        _deckRepositoryMock.Verify(x => x.GetDeckById(deckId), Times.Once);
        _cardRepositoryMock.Verify(x => x.GetCardsById(It.IsAny<IEnumerable<int>>()), Times.Never);
        _jokerRepositoryMock.Verify(x => x.GetJokersById(It.IsAny<IEnumerable<int>>()), Times.Never);
        _deckRepositoryMock.Verify(x => x.UpdateDeck(It.IsAny<Deck>()), Times.Never);
    }

    [Test]
    public async Task UpdateDeck_CardsRepositoryReturnsPartialList_UpdatesWithAvailableCards()
    {
        // Arrange
        var deckId = 1;
        var deckModel = new Deck
        {
            Id = deckId,
            Name = "Test Deck",
            Cards = new List<Card>(),
            Jokers = new List<Joker>()
        };

        var requestedCardIds = new List<int> { 1, 2, 3 };
        var availableCards = new List<Card>
        {
            new Card { Id = 1, Name = "Card 1" },
            new Card { Id = 3, Name = "Card 3" }
            // Card 2 is missing
        };

        var requestedJokerIds = new List<int> { 4, 5 };
        var availableJokers = new List<Joker>
        {
            new Joker { Id = 4, Name = "Joker 4" },
            new Joker { Id = 5, Name = "Joker 5" }
        };

        var deckDto = new DeckDto(deckModel)
        {
            Cards = requestedCardIds.Select(id => new CardDto { Id = id }).ToList(),
            Jokers = requestedJokerIds.Select(id => new JokerDto { Id = id }).ToList()
        };

        _deckRepositoryMock
            .Setup(x => x.GetDeckById(deckId))
            .ReturnsAsync(deckModel);

        _cardRepositoryMock
            .Setup(x => x.GetCardsById(requestedCardIds))
            .Returns(availableCards.BuildMock());

        _jokerRepositoryMock
            .Setup(x => x.GetJokersById(requestedJokerIds))
            .ReturnsAsync(availableJokers);

        _deckRepositoryMock
            .Setup(x => x.UpdateDeck(deckModel))
            .Returns(Task.CompletedTask);
        
        // Act
        var result = await _sut.UpdateDeck(deckDto);
        
        // Assert
        Assert.That(result, Is.True);
        Assert.That(deckModel.Cards.Count, Is.EqualTo(2)); // Only 2 cards found
        Assert.That(deckModel.Cards.Select(c => c.Id), Is.EquivalentTo(new[] { 1, 3 }));
        Assert.That(deckModel.Jokers.Count, Is.EqualTo(2));
    }

    #endregion

    #region DeleteDeck Tests

    [Test]
    public async Task DeleteDeck_ValidId_ReturnsTrue()
    {
        // Arrange
        var deckId = 1;
        var deckModel = new Deck
        {
            Id = deckId,
            Name = "Deck to Delete"
        };

        _deckRepositoryMock
            .Setup(x => x.GetDeckById(deckId))
            .ReturnsAsync(deckModel);

        _deckRepositoryMock
            .Setup(x => x.DeleteDeck(deckModel))
            .Returns(Task.CompletedTask);
        
        // Act
        var result = await _sut.DeleteDeck(deckId);
        
        // Assert
        Assert.That(result, Is.True);
        _deckRepositoryMock.Verify(x => x.GetDeckById(deckId), Times.Once);
        _deckRepositoryMock.Verify(x => x.DeleteDeck(deckModel), Times.Once);
    }

    [Test]
    public async Task DeleteDeck_InvalidId_ReturnsFalse()
    {
        // Arrange
        var deckId = 999;
        
        _deckRepositoryMock
            .Setup(x => x.GetDeckById(deckId))
            .ReturnsAsync((Deck?)null);
        
        // Act
        var result = await _sut.DeleteDeck(deckId);
        
        // Assert
        Assert.That(result, Is.False);
        _deckRepositoryMock.Verify(x => x.GetDeckById(deckId), Times.Once);
        _deckRepositoryMock.Verify(x => x.DeleteDeck(It.IsAny<Deck>()), Times.Never);
    }

    [Test]
    public async Task DeleteDeck_RepositoryThrowsException_PropagatesException()
    {
        // Arrange
        var deckId = 1;
        var deckModel = new Deck { Id = deckId, Name = "Deck" };

        _deckRepositoryMock
            .Setup(x => x.GetDeckById(deckId))
            .ReturnsAsync(deckModel);

        _deckRepositoryMock
            .Setup(x => x.DeleteDeck(deckModel))
            .ThrowsAsync(new InvalidOperationException("Database error"));
        
        // Act & Assert
        Assert.ThrowsAsync<InvalidOperationException>(async () => 
            await _sut.DeleteDeck(deckId));
        
        _deckRepositoryMock.Verify(x => x.GetDeckById(deckId), Times.Once);
        _deckRepositoryMock.Verify(x => x.DeleteDeck(deckModel), Times.Once);
    }

    #endregion

    #region Edge Cases and Integration Tests

    [Test]
    public async Task AddDeck_DuplicateDeckName_Succeeds()
    {
        // Arrange
        var creatureId = 1;
        var creature = new Creature { Id = creatureId, Name = "Creature" };
        var deckDto = new DeckDto { Name = "Duplicate Name" };

        var savedDeck = new Deck
        {
            Id = 1,
            Name = "Duplicate Name",
            Creatures = new List<Creature> { creature }
        };

        _creatureRepositoryMock
            .Setup(x => x.GetCreatureByIdAsync(creatureId))
            .ReturnsAsync(creature);

        _deckRepositoryMock
            .Setup(x => x.AddDeck(It.IsAny<Deck>()))
            .ReturnsAsync(savedDeck);
        
        // Act
        var result = await _sut.AddDeck(deckDto, creatureId);
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Name, Is.EqualTo("Duplicate Name"));
    }

    [Test]
    public async Task UpdateDeck_DeckNameNotUpdated_PreservesOriginalName()
    {
        // Arrange
        var deckId = 1;
        var originalName = "Original Name";
        var deckModel = new Deck
        {
            Id = deckId,
            Name = originalName,
            Cards = new List<Card>(),
            Jokers = new List<Joker>()
        };

        var deckDto = new DeckDto(deckModel)
        {
            Name = "Updated Name", // This should be ignored since UpdateDeck only updates cards and jokers
            Cards = new List<CardDto>(),
            Jokers = new List<JokerDto>()
        };

        _deckRepositoryMock
            .Setup(x => x.GetDeckById(deckId))
            .ReturnsAsync(deckModel);

        _cardRepositoryMock
            .Setup(x => x.GetCardsById(It.IsAny<IEnumerable<int>>()))
            .Returns(new List<Card>().BuildMock());

        _jokerRepositoryMock
            .Setup(x => x.GetJokersById(It.IsAny<IEnumerable<int>>()))
            .ReturnsAsync(new List<Joker>());

        _deckRepositoryMock
            .Setup(x => x.UpdateDeck(deckModel))
            .Returns(Task.CompletedTask);
        
        // Act
        var result = await _sut.UpdateDeck(deckDto);
        
        // Assert
        Assert.That(result, Is.True);
        // The deck name should remain unchanged since UpdateDeck only updates cards and jokers
        Assert.That(deckModel.Name, Is.EqualTo(originalName));
    }

    #endregion
}