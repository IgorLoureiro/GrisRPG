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
    
    public async Task<CreatureDto?> GetCreatureById(int id)
    {
        var creatureModel = await creatureRepository.GetCreatureByIdAsync(id);
        return creatureModel is null ? null : new CreatureDto(creatureModel);
    }

    public async Task<List<CreatureDto>> GetAllCreaturesByUserId(int userId)
    {
        return await creatureRepository.GetAllCreaturesFromUser(userId);
    }

    public async Task<bool> UpdateCreature(CreatureDto creatureDto)
    {
        var creatureModel = await creatureRepository.GetCreatureByIdAsync(creatureDto.Id);
        if (creatureModel is null)
            return false;
        
        creatureModel.Name = creatureDto.Name;

        await creatureRepository.UpdateCreature(creatureModel);
        return true;
    }

    public async Task<bool> DeleteCreatureById(int id)
    {
        var creature = await creatureRepository.GetCreatureByIdAsync(id);
        if (creature != null)
            await creatureRepository.DeleteCreature(creature);
        
        return (creature != null);
    }
}