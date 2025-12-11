using GrisAPI.DbContext;
using GrisAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GrisAPI.Repositories.CardRepository;

public class CardRepository(ApplicationDbContext context) : ICardRepository
{
    public async Task<List<Card>> GetCardsById(IEnumerable<int> ids)
    {
        return await context.Cards
            .Where(x => ids.Contains(x.Id))
            .ToListAsync();
    }
}