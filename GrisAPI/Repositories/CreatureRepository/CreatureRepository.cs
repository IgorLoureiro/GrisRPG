using GrisAPI.DbContext;
using GrisAPI.Models;

namespace GrisAPI.Repositories.CreatureRepository;

public sealed class CreatureRepository(ApplicationDbContext context) : ICreatureRepository
{
    public async Task<Creature> CreateAsync(Creature creature)
    {
        var createdCreature = await context.Creatures.AddAsync(creature);
        await context.SaveChangesAsync();
        return createdCreature.Entity;
    }

    public async Task<Creature?> GetCreatureByIdAsync(int id)
    {
        return await context.Creatures.FindAsync(id);
    }
}