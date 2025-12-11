using GrisAPI.Models;

namespace GrisAPI.Repositories.JokerRepository;

public interface IJokerRepository
{
    Task<List<Joker>> GetJokersById(IEnumerable<int> jokersId);
}