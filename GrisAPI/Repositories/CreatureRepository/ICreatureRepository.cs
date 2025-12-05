using GrisAPI.DTOs;
using GrisAPI.Models;

namespace GrisAPI.Repositories.CreatureRepository;

public interface ICreatureRepository
{
    Task<Creature> CreateAsync(Creature creature);
    Task<Creature?> GetCreatureByIdAsync(int id);
    Task<List<CreatureDto>> GetAllCreaturesFromUser(int userId);
    Task UpdateCreature(Creature creature);
    Task DeleteCreature(Creature creature);
}