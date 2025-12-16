using GrisAPI.DTOs;
using GrisAPI.Models;

namespace GrisAPI.Services.JokerService;

public interface IJokerService
{
    Task<List<Joker>> GetJokersById(List<int> ids);
    Task<Joker?> GetJokerById(int id);
    Task<List<Joker>> GetJokersByName(string name);
    Task<Joker?> AddJoker(JokerDto? jokerDto);
    Task<bool> UpdateJoker(JokerDto? jokerDto);
    Task<bool> DeleteJoker(int id);
}