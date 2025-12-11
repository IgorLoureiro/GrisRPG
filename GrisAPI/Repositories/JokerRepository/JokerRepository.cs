using GrisAPI.DbContext;
using GrisAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GrisAPI.Repositories.JokerRepository;

public sealed class JokerRepository(ApplicationDbContext context) : IJokerRepository
{
    public async Task<List<Joker>> GetJokersById(IEnumerable<int> jokersId)
    {
        return await context.Jokers
            .Where(x => jokersId.Contains(x.Id))
            .ToListAsync();
    }
}