using GrisAPI.DTOs;
using GrisAPI.Models;
using GrisAPI.Repositories.CreatureRepository;
using GrisAPI.Repositories.UserRepository;

namespace GrisAPI.Services.CreatureService;

public sealed class CreatureService(IUserRepository userRepository, ICreatureRepository creatureRepository)
    : ICreatureService
{
    public async Task<CreatureFilterResponse> GetFilteredCreatures(CreatureFilterRequest filter, int userId)
    {   
        var creatures = await creatureRepository.GetFilteredCreatures(filter, userId);
        var maxNumberOfPages = Convert.ToInt32(Math.Ceiling((double)creatures.Count / filter.Quantity));

        return new CreatureFilterResponse
        {
            Creatures = creatures,
            MaxNumberOfPages = maxNumberOfPages
        };
    }
    
    public async Task<CreatureDto> CreateCreature(string creatureName, int userId)
    {   
        var user = await userRepository.GetUserByIdAsync(userId);

        var creature = new Creature
        {
            Name = creatureName,
        };
        
        creature.Users.Add(user!);
        var creatureModel = await creatureRepository.CreateAsync(creature);

        return new CreatureDto(creatureModel);
    }
    
    public async Task<CreatureDto?> GetCreatureById(int id)
    {
        var creatureModel = await creatureRepository.GetCreatureByIdAsync(id);
        return creatureModel is null ? null : new CreatureDto(creatureModel);
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