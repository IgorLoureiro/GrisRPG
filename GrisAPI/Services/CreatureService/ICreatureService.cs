using GrisAPI.DTOs;

namespace GrisAPI.Services.CreatureService;

public interface ICreatureService
{
    Task<CreatureDto> CreateCreature(string creatureName, int userId);
    Task<List<CreatureDto>> GetAllCreaturesByUserId(int userId);
    Task<CreatureDto?> GetCreatureById(int id);
    Task<bool> UpdateCreature(CreatureDto creatureDto);
    Task<bool> DeleteCreatureById(int id);
}