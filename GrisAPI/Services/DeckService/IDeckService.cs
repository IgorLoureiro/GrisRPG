using GrisAPI.DTOs;
using GrisAPI.Models;

namespace GrisAPI.Services.DeckService;

public interface IDeckService
{
    Task<DeckDto?> GetDeckById(int id);
    Task<DeckDto?> AddDeck(DeckDto deck, int creatureId);
    Task<bool> UpdateDeck(DeckDto deck);
    Task<bool> DeleteDeck(int id);
}