using GrisAPI.DTOs;
using GrisAPI.Models;
using GrisAPI.Repositories.JokerRepository;

namespace GrisAPI.Services.JokerService;

public class JokerService(IJokerRepository jokerRepository) : IJokerService
{
    public async Task<List<Joker>> GetJokersById(List<int> ids)
    {
        return await jokerRepository.GetJokersById(ids);
    }

    public async Task<Joker?> GetJokerById(int id)
    {
        return await jokerRepository.GetJokerById(id);
    }

    public async Task<List<Joker>> GetJokersByName(string name)
    {
        return await jokerRepository.GetJokersByName(name);
    }

    public async Task<Joker?> AddJoker(JokerDto? jokerDto)
    {
        if (jokerDto is null)
            return null;

        var jokerModel = new Joker
        {
            Name = jokerDto.Name,
            Description = jokerDto.Description,
        };
        
        return await jokerRepository.AddJoker(jokerModel);
    }

    public async Task<bool> UpdateJoker(JokerDto? jokerDto)
    {
        if (jokerDto is null)
            return false;
        
        var jokerModel = await jokerRepository.GetJokerById(jokerDto.Id);

        if (jokerModel is null)
            return false;
        
        jokerModel.Name = jokerDto.Name;
        jokerModel.Description = jokerDto.Description;
        
        await jokerRepository.UpdateJoker(jokerModel);
        return true;
    }

    public async Task<bool> DeleteJoker(int id)
    {
        var jokerModel = await jokerRepository.GetJokerById(id);

        if (jokerModel is null)
            return false;
        
        await jokerRepository.DeleteJoker(jokerModel);
        return true;
    }
}