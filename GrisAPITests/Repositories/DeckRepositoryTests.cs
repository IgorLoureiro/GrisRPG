using System.Diagnostics.CodeAnalysis;
using GrisAPI.DbContext;
using GrisAPI.Models;
using GrisAPI.Repositories.DeckRepository;
using Microsoft.EntityFrameworkCore;

namespace GrisAPITests.Repositories;

[TestFixture]
[ExcludeFromCodeCoverage]
public sealed class DeckRepositoryTests
{
    private ApplicationDbContext _dbContext = null!;
    private DeckRepository _sut = null!;

    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "DeckRepositoryTests_DB")
            .Options;

        _dbContext = new ApplicationDbContext(options);
        _dbContext.Database.EnsureDeleted();
        _dbContext.Database.EnsureCreated();
        
        _sut = new DeckRepository(_dbContext);
    }
    
    [TearDown]
    public void TearDown()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    [Test]
    public async Task GetDeckById_WithValidId_ReturnsDeck()
    {
        // Arrange
        var deck = new Deck
        {
            Id = 1,
            Name = "Test Deck",
            Creatures = new List<Creature>
            {
                new Creature { Id = 1, Name = "Test Creature" }
            }
        };
        
        await _dbContext.Decks.AddAsync(deck);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _sut.GetDeckById(deck.Id);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Id, Is.EqualTo(deck.Id));
        Assert.That(result.Name, Is.EqualTo(deck.Name));
        Assert.That(result.Creatures, Has.Count.EqualTo(1));
    }

    [Test]
    public async Task GetDeckById_WithValidIdWithCardsAndJokers_ReturnsDeckWithRelatedData()
    {
        // Arrange
        var deck = new Deck
        {
            Id = 1,
            Name = "Test Deck",
            Cards = new List<Card>
            {
                new Card { Id = 1, Name = "Card 1" },
                new Card { Id = 2, Name = "Card 2" }
            },
            Jokers = new List<Joker>
            {
                new Joker { Id = 1, Name = "Joker 1" }
            }
        };
        
        await _dbContext.Decks.AddAsync(deck);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _sut.GetDeckById(deck.Id);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Cards, Has.Count.EqualTo(2));
        Assert.That(result.Jokers, Has.Count.EqualTo(1));
    }

    [Test]
    public async Task GetDeckById_WithInvalidId_ReturnsNull()
    {
        // Arrange
        var deck = new Deck { Id = 1, Name = "Test Deck" };
        await _dbContext.Decks.AddAsync(deck);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _sut.GetDeckById(999);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetDeckById_WithZeroId_ReturnsNull()
    {
        // Arrange
        // Deck IDs usually start at 1, 0 is invalid

        // Act
        var result = await _sut.GetDeckById(0);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetDeckById_WithNegativeId_ReturnsNull()
    {
        // Arrange
        // Negative IDs are invalid

        // Act
        var result = await _sut.GetDeckById(-1);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetDeckById_AfterDeckIsDeleted_ReturnsNull()
    {
        // Arrange
        var deck = new Deck { Id = 1, Name = "Test Deck" };
        await _dbContext.Decks.AddAsync(deck);
        await _dbContext.SaveChangesAsync();
        
        _dbContext.Decks.Remove(deck);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _sut.GetDeckById(deck.Id);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task AddDeck_ValidDeck_CreatesDeckSuccessfully()
    {
        // Arrange
        var deck = new Deck
        {
            Name = "New Deck",
            Creatures = new List<Creature>
            {
                new Creature { Id = 1, Name = "Creature 1" }
            },
            Cards = new List<Card>
            {
                new Card { Id = 1, Name = "Card 1" }
            }
        };

        // Act
        var result = await _sut.AddDeck(deck);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.GreaterThan(0));
        Assert.That(result.Name, Is.EqualTo("New Deck"));
        Assert.That(result.Creatures, Has.Count.EqualTo(1));
        Assert.That(result.Cards, Has.Count.EqualTo(1));
        
        // Verify in database
        var dbDeck = await _dbContext.Decks.FindAsync(result.Id);
        Assert.That(dbDeck, Is.Not.Null);
        Assert.That(dbDeck!.Name, Is.EqualTo("New Deck"));
    }

    [Test]
    public async Task AddDeck_DeckWithNullName_ThrowsException()
    {
        // Arrange
        var deck = new Deck
        {
            Name = null!, // This will cause validation error
            Creatures = new List<Creature>()
        };

        // Act & Assert
        var ex = Assert.ThrowsAsync<DbUpdateException>(async () => 
            await _sut.AddDeck(deck));
        Assert.That(ex, Is.Not.Null);
    }

    [Test]
    public async Task AddDeck_DeckWithExistingCreature_AssociatesCorrectly()
    {
        // Arrange
        var creature = new Creature { Id = 1, Name = "Existing Creature" };
        await _dbContext.Creatures.AddAsync(creature);
        await _dbContext.SaveChangesAsync();

        var deck = new Deck
        {
            Name = "New Deck",
            Creatures = new List<Creature> { creature }
        };

        // Act
        var result = await _sut.AddDeck(deck);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Creatures.First().Id, Is.EqualTo(creature.Id));
        
        // Verify the relationship in database
        var dbDeck = await _dbContext.Decks
            .Include(d => d.Creatures)
            .FirstOrDefaultAsync(d => d.Id == result.Id);
        Assert.That(dbDeck!.Creatures, Has.Count.EqualTo(1));
        Assert.That(dbDeck.Creatures.First().Name, Is.EqualTo("Existing Creature"));
    }

    [Test]
    public async Task UpdateDeck_NonExistentDeck_ThrowsException()
    {
        // Arrange
        var deck = new Deck
        {
            Id = 999, // Non-existent
            Name = "Non-existent Deck"
        };

        // Act & Assert
        var ex = Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () => 
            await _sut.UpdateDeck(deck));
        Assert.That(ex, Is.Not.Null);
    }

    [Test]
    public async Task DeleteDeck_ValidDeck_DeletesSuccessfully()
    {
        // Arrange
        var deck = new Deck
        {
            Id = 1,
            Name = "Deck to Delete",
            Cards = new List<Card>
            {
                new Card { Id = 1, Name = "Card" }
            }
        };
        
        await _dbContext.Decks.AddAsync(deck);
        await _dbContext.SaveChangesAsync();

        // Act
        await _sut.DeleteDeck(deck);

        // Assert
        var dbDeck = await _dbContext.Decks.FindAsync(deck.Id);
        Assert.That(dbDeck, Is.Null);
        
        // Verify related data is also deleted if cascade delete is configured
        var cardsStillExist = await _dbContext.Cards.AnyAsync(c => c.Id == 1);
        Assert.That(cardsStillExist, Is.True); // Cards should still exist (no cascade delete)
    }

    [Test]
    public async Task DeleteDeck_DeckWithMultipleRelationships_DeletesSuccessfully()
    {
        // Arrange
        var deck = new Deck
        {
            Id = 1,
            Name = "Complex Deck",
            Creatures = new List<Creature>
            {
                new Creature { Id = 1, Name = "Creature" }
            },
            Cards = new List<Card>
            {
                new Card { Id = 1, Name = "Card" }
            },
            Jokers = new List<Joker>
            {
                new Joker { Id = 1, Name = "Joker" }
            }
        };
        
        await _dbContext.Decks.AddAsync(deck);
        await _dbContext.SaveChangesAsync();

        // Act
        await _sut.DeleteDeck(deck);

        // Assert
        var dbDeck = await _dbContext.Decks.FindAsync(deck.Id);
        Assert.That(dbDeck, Is.Null);
        
        // Verify relationships are properly handled
        var creatureStillExists = await _dbContext.Creatures.AnyAsync(c => c.Id == 1);
        Assert.That(creatureStillExists, Is.True);
    }

    [Test]
    public async Task DeleteDeck_AlreadyDeletedDeck_ThrowsException()
    {
        // Arrange
        var deck = new Deck { Id = 1, Name = "Deck" };
        await _dbContext.Decks.AddAsync(deck);
        await _dbContext.SaveChangesAsync();
        
        // Delete it once
        await _sut.DeleteDeck(deck);
        
        // Try to delete again (deck is already detached/deleted)
        _dbContext.Entry(deck).State = EntityState.Detached;

        // Act & Assert
        var ex = Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () => 
            await _sut.DeleteDeck(deck));
        Assert.That(ex, Is.Not.Null);
    }

    [Test]
    public async Task DeleteDeck_DeckWithNullProperties_HandlesGracefully()
    {
        // Arrange
        var deck = new Deck
        {
            Id = 1,
            Name = "Deck",
            Cards = null,
            Jokers = null,
            Creatures = null
        };
        
        await _dbContext.Decks.AddAsync(deck);
        await _dbContext.SaveChangesAsync();

        // Act
        await _sut.DeleteDeck(deck);

        // Assert
        var dbDeck = await _dbContext.Decks.FindAsync(deck.Id);
        Assert.That(dbDeck, Is.Null);
    }

    [Test]
    public async Task AddDeck_WithTracking_ContextTracksEntity()
    {
        // Arrange
        var deck = new Deck { Name = "Tracked Deck" };

        // Act
        var result = await _sut.AddDeck(deck);

        // Assert
        var isTracked = _dbContext.ChangeTracker.Entries<Deck>()
            .Any(e => e.Entity.Id == result.Id);
        Assert.That(isTracked, Is.True);
    }
}