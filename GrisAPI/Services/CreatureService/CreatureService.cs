using GrisAPI.DTOs;
using GrisAPI.Models;
using GrisAPI.Repositories.CreatureRepository;
using GrisAPI.Repositories.UserRepository;

namespace GrisAPI.Services.CreatureService;

public sealed class CreatureService(IUserRepository userRepository, ICreatureRepository creatureRepository)
    : ICreatureService
{
    public async Task<CreatureDto> CreateCreature(string creatureName, int userId)
    {   
        var user = await userRepository.GetUserByIdAsync(userId);

        var creature = new Creature
        {
            Name = creatureName,
            ExtraDeck = new ExtraDeck(),
        };
        
        creature.Users.Add(user!);
        var creatureModel = await creatureRepository.CreateAsync(creature);

        return new CreatureDto()
        {
            Name = creatureModel.Name,
            ExtraDeck = creatureModel.ExtraDeck,
            Id = creatureModel.Id,
            ExtraDeckId = creatureModel.ExtraDeckId,
        };
    }
}