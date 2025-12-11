using GrisAPI.Models;

namespace GrisAPI.Repositories.CardRepository;

public interface ICardRepository
{
    Task<List<Card>> GetCardsById(IEnumerable<int> ids);
}