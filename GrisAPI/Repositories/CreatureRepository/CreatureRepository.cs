using GrisAPI.DbContext;
using GrisAPI.DTOs;
using GrisAPI.Models;
using Microsoft.EntityFrameworkCore;

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

    public async Task<List<CreatureDto>> GetFilteredCreatures(CreatureFilterRequest request, int userId)
    {
         return await context.Creatures
            .Where(x => x.Users.Any(u => u.Id == userId))
            .Where(x => string.IsNullOrWhiteSpace(request.Name) || x.Name.Contains(request.Name))
            .Skip(request.CurrentPage * request.Quantity)
            .Take(request.Quantity)
            .OrderBy(x => x.Name)
            .Select(x => new CreatureDto(x))
            .ToListAsync();
    }

    public async Task UpdateCreature(Creature creature)
    {
        context.Creatures.Update(creature);
        await context.SaveChangesAsync();
    }
    
    public async Task DeleteCreature(Creature creature)
    {
        context.Creatures.Remove(creature);
        await context.SaveChangesAsync();
    }
}