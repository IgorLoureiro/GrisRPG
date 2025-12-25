using GrisAPI.DbContext;
using GrisAPI.Models;

namespace GrisAPI.Repositories.DeckRepository;

public sealed class DeckRepository(ApplicationDbContext context) : IDeckRepository
{
    public async Task<Deck?> GetDeckById(int id)
    {
        return await context.Decks.FindAsync(id);
    }

    public async Task<Deck> AddDeck(Deck deck)
    {
        var addedDeck = await context.Decks.AddAsync(deck);
        await context.SaveChangesAsync();
        return addedDeck.Entity;
    }

    public async Task UpdateDeck(Deck deck)
    {
        context.Decks.Update(deck);
        await context.SaveChangesAsync();
    }

    public async Task DeleteDeck(Deck deck)
    {
        context.Decks.Remove(deck);
        await context.SaveChangesAsync();
    }
}