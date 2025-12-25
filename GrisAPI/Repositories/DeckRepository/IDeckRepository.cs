using GrisAPI.Models;

namespace GrisAPI.Repositories.DeckRepository;

public interface IDeckRepository
{
    Task<Deck?> GetDeckById(int id);
    Task<Deck> AddDeck(Deck deck);
    Task UpdateDeck(Deck deck);
    Task DeleteDeck(Deck deck);
}