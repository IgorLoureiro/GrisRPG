using GrisAPI.DTOs;

namespace GrisAPI.Services.CreatureService;

public interface ICreatureService
{
    Task<CreatureFilterResponse> GetFilteredCreatures(CreatureFilterRequest filter, int userId);
    Task<CreatureDto> CreateCreature(string creatureName, int userId);
    Task<CreatureDto?> GetCreatureById(int id);
    Task<bool> UpdateCreature(CreatureDto creatureDto);
    Task<bool> DeleteCreatureById(int id);
}