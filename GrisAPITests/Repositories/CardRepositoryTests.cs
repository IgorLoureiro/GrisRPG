using System.Diagnostics.CodeAnalysis;
using GrisAPI.DbContext;
using GrisAPI.DTOs;
using GrisAPI.Models;
using GrisAPI.Models.Enums;
using GrisAPI.Repositories.CardRepository;
using Microsoft.EntityFrameworkCore;

namespace GrisAPITests.Repositories;

[TestFixture]
[ExcludeFromCodeCoverage]
public class CardRepositoryTests
{
    private ApplicationDbContext _dbContext = null!;
    private CardRepository _sut = null!;

    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "CardRepositoryTests_DB")
            .Options;

        _dbContext = new ApplicationDbContext(options);
        _dbContext.Database.EnsureDeleted();
        _dbContext.Database.EnsureCreated();
        
        _sut = new CardRepository(_dbContext);
    }
    
    [TearDown]
    public void TearDown()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    [Test]
    public void GetFilteredCards_WithNameFilter_ReturnsFilteredCards()
    {
        // Arrange
        var cards = new List<Card>
        {
            new() { Id = 1, Name = "Fire Dragon", Symbol = Symbol.Hearts, Manifestation = Manifestation.Fire },
            new() { Id = 2, Name = "Water Dragon", Symbol = Symbol.Clubs, Manifestation = Manifestation.Water },
            new() { Id = 3, Name = "Fire Phoenix", Symbol = Symbol.Hearts, Manifestation = Manifestation.Fire }
        };
        
        _dbContext.Cards.AddRange(cards);
        _dbContext.SaveChanges();

        var filterRequest = new CardFilterRequest
        {
            Name = "Dragon",
            Quantity = 10,
            CurrentPage = 0
        };

        // Act
        var results = _sut.GetFilteredCards(filterRequest).ToList();

        // Assert
        Assert.That(results, Is.Not.Null);
        Assert.That(results.Count, Is.EqualTo(2));
        Assert.That(results.All(c => c.Name.Contains("Dragon")), Is.True);
    }

    [Test]
    public void GetFilteredCards_WithSymbolFilter_ReturnsFilteredCards()
    {
        // Arrange
        var cards = new List<Card>
        {
            new() { Id = 1, Name = "Fire Dragon", Symbol = Symbol.Hearts, Manifestation = Manifestation.Fire },
            new() { Id = 2, Name = "Water Dragon", Symbol = Symbol.Clubs, Manifestation = Manifestation.Water },
            new() { Id = 3, Name = "Fire Phoenix", Symbol = Symbol.Hearts, Manifestation = Manifestation.Fire }
        };
        
        _dbContext.Cards.AddRange(cards);
        _dbContext.SaveChanges();

        var filterRequest = new CardFilterRequest
        {
            Symbol = Symbol.Hearts,
            Quantity = 10,
            CurrentPage = 0
        };

        // Act
        var results = _sut.GetFilteredCards(filterRequest).ToList();

        // Assert
        Assert.That(results, Is.Not.Null);
        Assert.That(results.Count, Is.EqualTo(2));
        Assert.That(results.All(c => c.Symbol == Symbol.Hearts), Is.True);
    }

    [Test]
    public void GetFilteredCards_WithManifestationFilter_ReturnsFilteredCards()
    {
        // Arrange
        var cards = new List<Card>
        {
            new() { Id = 1, Name = "Fire Dragon", Symbol = Symbol.Hearts, Manifestation = Manifestation.Fire },
            new() { Id = 2, Name = "Water Dragon", Symbol = Symbol.Clubs, Manifestation = Manifestation.Water },
            new() { Id = 3, Name = "Fire Phoenix", Symbol = Symbol.Hearts, Manifestation = Manifestation.Fire }
        };
        
        _dbContext.Cards.AddRange(cards);
        _dbContext.SaveChanges();

        var filterRequest = new CardFilterRequest
        {
            Manifestation = Manifestation.Fire,
            Quantity = 10,
            CurrentPage = 0
        };

        // Act
        var results = _sut.GetFilteredCards(filterRequest).ToList();

        // Assert
        Assert.That(results, Is.Not.Null);
        Assert.That(results.Count, Is.EqualTo(2));
        Assert.That(results.All(c => c.Manifestation == Manifestation.Fire), Is.True);
    }

    [Test]
    public void GetFilteredCards_WithPagination_ReturnsCorrectPage()
    {
        // Arrange
        var cards = Enumerable.Range(1, 25)
            .Select(i => new Card { Id = i, Name = $"Card {i}", Symbol = Symbol.Hearts, Manifestation = Manifestation.Fire })
            .ToList();
        
        _dbContext.Cards.AddRange(cards);
        _dbContext.SaveChanges();

        var filterRequest = new CardFilterRequest
        {
            Quantity = 10,
            CurrentPage = 1 // Second page
        };

        // Act
        var results = _sut.GetFilteredCards(filterRequest).ToList();

        // Assert
        Assert.That(results, Is.Not.Null);
        Assert.That(results.Count, Is.EqualTo(10));
        Assert.That(results.First().Name, Is.EqualTo("Card 11")); // Skip first 10
    }

    [Test]
    public void GetFilteredCards_OrdersByName_ReturnsSortedResults()
    {
        // Arrange
        var cards = new List<Card>
        {
            new() { Id = 1, Name = "Zebra", Symbol = Symbol.Hearts, Manifestation = Manifestation.Fire },
            new() { Id = 2, Name = "Apple", Symbol = Symbol.Hearts, Manifestation = Manifestation.Fire },
            new() { Id = 3, Name = "Monkey", Symbol = Symbol.Hearts, Manifestation = Manifestation.Fire }
        };
        
        _dbContext.Cards.AddRange(cards);
        _dbContext.SaveChanges();

        var filterRequest = new CardFilterRequest
        {
            Quantity = 10,
            CurrentPage = 0
        };

        // Act
        var results = _sut.GetFilteredCards(filterRequest).ToList();

        // Assert
        Assert.That(results, Is.Not.Null);
        Assert.That(results[0].Name, Is.EqualTo("Apple"));
        Assert.That(results[1].Name, Is.EqualTo("Monkey"));
        Assert.That(results[2].Name, Is.EqualTo("Zebra"));
    }

    [Test]
    public void GetCardsById_WithValidIds_ReturnsMatchingCards()
    {
        // Arrange
        var cards = new List<Card>
        {
            new() { Id = 1, Name = "Card 1", Symbol = Symbol.Hearts, Manifestation = Manifestation.Fire },
            new() { Id = 2, Name = "Card 2", Symbol = Symbol.Hearts, Manifestation = Manifestation.Fire },
            new() { Id = 3, Name = "Card 3", Symbol = Symbol.Hearts, Manifestation = Manifestation.Fire }
        };
        
        _dbContext.Cards.AddRange(cards);
        _dbContext.SaveChanges();

        var ids = new List<int> { 1, 3 };

        // Act
        var results = _sut.GetCardsById(ids).ToList();

        // Assert
        Assert.That(results, Is.Not.Null);
        Assert.That(results.Count, Is.EqualTo(2));
        Assert.That(results.Select(c => c.Id), Is.EquivalentTo(ids));
    }

    [Test]
    public void GetCardsById_WithNonExistentIds_ReturnsEmpty()
    {
        // Arrange
        var cards = new List<Card>
        {
            new() { Id = 1, Name = "Card 1", Symbol = Symbol.Hearts, Manifestation = Manifestation.Fire }
        };
        
        _dbContext.Cards.AddRange(cards);
        _dbContext.SaveChanges();

        var ids = new List<int> { 99, 100 };

        // Act
        var results = _sut.GetCardsById(ids).ToList();

        // Assert
        Assert.That(results, Is.Not.Null);
        Assert.That(results, Is.Empty);
    }

    [Test]
    public async Task GetCardById_WithValidId_ReturnsCard()
    {
        // Arrange
        var card = new Card
        {
            Id = 5,
            Name = "Test Card",
            Symbol = Symbol.Diamonds,
            Manifestation = Manifestation.Earth
        };
        
        _dbContext.Cards.Add(card);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _sut.GetCardById(card.Id);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Id, Is.EqualTo(card.Id));
        Assert.That(result.Name, Is.EqualTo(card.Name));
        Assert.That(result.Symbol, Is.EqualTo(card.Symbol));
        Assert.That(result.Manifestation, Is.EqualTo(card.Manifestation));
    }

    [Test]
    public async Task GetCardById_WithInvalidId_ReturnsNull()
    {
        // Act
        var result = await _sut.GetCardById(999);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task AddCard_ValidCard_CreatesSuccessfully()
    {
        // Arrange
        var card = new Card
        {
            Name = "New Card",
            Symbol = Symbol.Spades,
            Manifestation = Manifestation.Earth
        };

        // Act
        var result = await _sut.AddCard(card);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.GreaterThan(0));
        Assert.That(result.Name, Is.EqualTo(card.Name));
        Assert.That(result.Symbol, Is.EqualTo(card.Symbol));
        Assert.That(result.Manifestation, Is.EqualTo(card.Manifestation));
        
        // Verify it was saved to the database
        var dbCard = await _dbContext.Cards.FindAsync(result.Id);
        Assert.That(dbCard, Is.Not.Null);
        Assert.That(dbCard!.Name, Is.EqualTo(card.Name));
    }

    [Test]
    public async Task UpdateCard_ValidCard_UpdatesSuccessfully()
    {
        // Arrange
        var card = new Card
        {
            Id = 1,
            Name = "Original Name",
            Symbol = Symbol.Hearts,
            Manifestation = Manifestation.Fire
        };
        
        _dbContext.Cards.Add(card);
        await _dbContext.SaveChangesAsync();

        // Act
        card.Name = "Updated Name";
        card.Symbol = Symbol.Diamonds;
        card.Manifestation = Manifestation.Water;
        await _sut.UpdateCard(card);

        // Assert
        var updatedCard = await _dbContext.Cards.FindAsync(card.Id);
        Assert.That(updatedCard, Is.Not.Null);
        Assert.That(updatedCard!.Name, Is.EqualTo("Updated Name"));
        Assert.That(updatedCard.Symbol, Is.EqualTo(Symbol.Diamonds));
        Assert.That(updatedCard.Manifestation, Is.EqualTo(Manifestation.Water));
    }

    [Test]
    public async Task DeleteCard_ValidCard_DeletesSuccessfully()
    {
        // Arrange
        var card = new Card
        {
            Id = 1,
            Name = "Card to Delete",
            Symbol = Symbol.Hearts,
            Manifestation = Manifestation.Fire
        };
        
        _dbContext.Cards.Add(card);
        await _dbContext.SaveChangesAsync();

        // Act
        await _sut.DeleteCard(card);

        // Assert
        var deletedCard = await _dbContext.Cards.FindAsync(card.Id);
        Assert.That(deletedCard, Is.Null);
    }

    [Test]
    public void GetFilteredCards_WithAllFilters_ReturnsCorrectlyFilteredCards()
    {
        // Arrange
        var cards = new List<Card>
        {
            new() { Id = 1, Name = "Fire Dragon", Symbol = Symbol.Hearts, Manifestation = Manifestation.Fire },
            new() { Id = 2, Name = "Fire Wyrm", Symbol = Symbol.Hearts, Manifestation = Manifestation.Fire },
            new() { Id = 3, Name = "Water Dragon", Symbol = Symbol.Clubs, Manifestation = Manifestation.Water },
            new() { Id = 4, Name = "Fire Phoenix", Symbol = Symbol.Hearts, Manifestation = Manifestation.Fire }
        };
        
        _dbContext.Cards.AddRange(cards);
        _dbContext.SaveChanges();

        var filterRequest = new CardFilterRequest
        {
            Name = "Dragon",
            Symbol = Symbol.Hearts,
            Manifestation = Manifestation.Fire,
            Quantity = 10,
            CurrentPage = 0
        };

        // Act
        var results = _sut.GetFilteredCards(filterRequest).ToList();

        // Assert
        Assert.That(results, Is.Not.Null);
        Assert.That(results.Count, Is.EqualTo(1)); // Only Fire Dragon matches all filters
        Assert.That(results[0].Id, Is.EqualTo(1));
        Assert.That(results[0].Name, Is.EqualTo("Fire Dragon"));
        Assert.That(results[0].Symbol, Is.EqualTo(Symbol.Hearts));
        Assert.That(results[0].Manifestation, Is.EqualTo(Manifestation.Fire));
    }

    [Test]
    public void GetFilteredCards_WithEmptyFilters_ReturnsAllCards()
    {
        // Arrange
        var cards = new List<Card>
        {
            new() { Id = 1, Name = "Card 1", Symbol = Symbol.Hearts, Manifestation = Manifestation.Fire },
            new() { Id = 2, Name = "Card 2", Symbol = Symbol.Clubs, Manifestation = Manifestation.Water },
            new() { Id = 3, Name = "Card 3", Symbol = Symbol.Diamonds, Manifestation = Manifestation.Earth }
        };
        
        _dbContext.Cards.AddRange(cards);
        _dbContext.SaveChanges();

        var filterRequest = new CardFilterRequest
        {
            Quantity = 10,
            CurrentPage = 0
        };

        // Act
        var results = _sut.GetFilteredCards(filterRequest).ToList();

        // Assert
        Assert.That(results, Is.Not.Null);
        Assert.That(results.Count, Is.EqualTo(3));
    }

    [Test]
    public void GetFilteredCards_WithNullEnumFilters_ReturnsAllCardsIgnoringThoseFilters()
    {
        // Arrange
        var cards = new List<Card>
        {
            new() { Id = 1, Name = "Fire Dragon", Symbol = Symbol.Hearts, Manifestation = Manifestation.Fire },
            new() { Id = 2, Name = "Water Dragon", Symbol = Symbol.Clubs, Manifestation = Manifestation.Water },
            new() { Id = 3, Name = "Earth Dragon", Symbol = Symbol.Diamonds, Manifestation = Manifestation.Earth }
        };
        
        _dbContext.Cards.AddRange(cards);
        _dbContext.SaveChanges();

        var filterRequest = new CardFilterRequest
        {
            Name = "Dragon",
            // Symbol is null
            // Manifestation is null
            Quantity = 10,
            CurrentPage = 0
        };

        // Act
        var results = _sut.GetFilteredCards(filterRequest).ToList();

        // Assert
        Assert.That(results, Is.Not.Null);
        Assert.That(results.Count, Is.EqualTo(3));
        Assert.That(results.All(c => c.Name.Contains("Dragon")), Is.True);
    }

    [Test]
    public void GetFilteredCards_WithZeroQuantity_ReturnsEmptyList()
    {
        // Arrange
        var cards = new List<Card>
        {
            new() { Id = 1, Name = "Card 1", Symbol = Symbol.Hearts, Manifestation = Manifestation.Fire },
            new() { Id = 2, Name = "Card 2", Symbol = Symbol.Hearts, Manifestation = Manifestation.Fire }
        };
        
        _dbContext.Cards.AddRange(cards);
        _dbContext.SaveChanges();

        var filterRequest = new CardFilterRequest
        {
            Quantity = 0,
            CurrentPage = 0
        };

        // Act
        var results = _sut.GetFilteredCards(filterRequest).ToList();

        // Assert
        Assert.That(results, Is.Not.Null);
        Assert.That(results, Is.Empty);
    }

    [Test]
    public void GetFilteredCards_WithDifferentEnumValues_HandlesAllEnumsCorrectly()
    {
        // Arrange - Test various enum values
        var cards = new List<Card>
        {
            new() { Id = 1, Name = "Fire Attack", Symbol = Symbol.Hearts, Manifestation = Manifestation.Assault },
            new() { Id = 2, Name = "Wind Command", Symbol = Symbol.Spades, Manifestation = Manifestation.Command },
            new() { Id = 3, Name = "Poison Plant", Symbol = Symbol.Diamonds, Manifestation = Manifestation.Plant },
            new() { Id = 4, Name = "Metal Artillery", Symbol = Symbol.Clubs, Manifestation = Manifestation.Artillery }
        };
        
        _dbContext.Cards.AddRange(cards);
        _dbContext.SaveChanges();

        // Test each enum filter
        var filterRequest = new CardFilterRequest
        {
            Manifestation = Manifestation.Command,
            Quantity = 10,
            CurrentPage = 0
        };

        // Act
        var results = _sut.GetFilteredCards(filterRequest).ToList();

        // Assert
        Assert.That(results, Is.Not.Null);
        Assert.That(results.Count, Is.EqualTo(1));
        Assert.That(results[0].Manifestation, Is.EqualTo(Manifestation.Command));
        Assert.That(results[0].Name, Is.EqualTo("Wind Command"));
    }
}