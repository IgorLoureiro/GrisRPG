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

    public async Task<Joker?> GetJokerById(int id)
    {
        return await context.Jokers.FindAsync(id);
    }

    public async Task<List<Joker>> GetJokersByName(string name)
    {
        return await context.Jokers
            .Where(x => x.Name.Contains(name))
            .ToListAsync();
    }

    public async Task<Joker> AddJoker(Joker joker)
    {
        var createdJoker = await context.Jokers.AddAsync(joker);
        await context.SaveChangesAsync();
        return createdJoker.Entity;
    }

    public async Task UpdateJoker(Joker joker)
    {
        context.Jokers.Update(joker);
        await context.SaveChangesAsync();
    }

    public async Task DeleteJoker(Joker joker)
    {
        context.Jokers.Remove(joker);
        await context.SaveChangesAsync();
    }
}