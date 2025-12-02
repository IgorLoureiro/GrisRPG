using GrisAPI.DTOs;

namespace GrisAPI.Services.CreatureService;

public interface ICreatureService
{
    Task<CreatureDto> CreateCreature(string creatureName, int userId);
}