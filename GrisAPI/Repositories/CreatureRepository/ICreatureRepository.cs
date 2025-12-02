using GrisAPI.Models;

namespace GrisAPI.Repositories.CreatureRepository;

public interface ICreatureRepository
{
    Task<Creature> CreateAsync(Creature creature);
    Task<Creature?> GetCreatureByIdAsync(int id);
}