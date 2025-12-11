using GrisAPI.DbContext;
using GrisAPI.Models;

namespace GrisAPI.Repositories.ExtraDeckRepository;

public sealed class ExtraDeckRepository(ApplicationDbContext context) : IExtraDeckRepository
{
    public async Task<ExtraDeck?> GetExtraDeckById(int id)
    {
        return await context.ExtraDecks.FindAsync(id);
    }

    public async Task UpdateExtraDeck (ExtraDeck extraDeck)
    {
        context.ExtraDecks.Update(extraDeck);
        await context.SaveChangesAsync();
    }
}