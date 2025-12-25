using GrisAPI.DbContext;
using GrisAPI.DTOs;
using GrisAPI.Models;

namespace GrisAPI.Repositories.CardRepository;

public class CardRepository(ApplicationDbContext context) : ICardRepository
{
    public IQueryable<Card> GetFilteredCards(CardFilterRequest filterRequest)
    {
        return context.Cards
            .Where(x => string.IsNullOrWhiteSpace(filterRequest.Name) || x.Name.Contains(filterRequest.Name))
            .Where(x => filterRequest.Symbol == null || x.Symbol == filterRequest.Symbol)
            .Where(x => filterRequest.Manifestation == null || x.Manifestation == filterRequest.Manifestation)
            .Skip(filterRequest.CurrentPage * filterRequest.Quantity)
            .Take(filterRequest.Quantity)
            .OrderBy(x => x.Name);
    }
    
    public IQueryable<Card> GetCardsById(IEnumerable<int> ids)
    {
        return context.Cards
            .Where(x => ids.Contains(x.Id));
    }
    
    public async Task<Card?> GetCardById(int id)
    {
        return await context.Cards.FindAsync(id);
    }

    public async Task<Card> AddCard(Card card)
    {
        var createdCard = await context.Cards.AddAsync(card);
        await context.SaveChangesAsync();
        return createdCard.Entity;
    }

    public async Task UpdateCard(Card card)
    {
        context.Cards.Update(card);
        await context.SaveChangesAsync();
    }

    public async Task DeleteCard(Card card)
    {
        context.Cards.Remove(card);
        await context.SaveChangesAsync();
    }
}