using GrisAPI.Models;

namespace GrisAPI.Repositories.JokerRepository;

public interface IJokerRepository
{
    Task<List<Joker>> GetJokersById(IEnumerable<int> jokersId);
    Task<Joker?> GetJokerById(int id);
    Task<List<Joker>> GetJokersByName(string name);
    Task<Joker> AddJoker(Joker joker);
    Task UpdateJoker(Joker joker);
    Task DeleteJoker(Joker joker);
}