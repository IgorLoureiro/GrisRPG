using GrisAPI.DTOs;
using GrisAPI.Models;

namespace GrisAPI.Services.CardService;

public interface ICardService
{
    Task<CardFilterResponse> GetFilteredCards(CardFilterRequest filterRequest);
    Task<List<CardDto>> GetCardsById(List<int> ids);
    Task<Card?> GetCardById(int id);
    Task<Card?> AddCard(CardDto? cardDto);
    Task<bool> UpdateCard(CardDto? cardDto);
    Task<bool> DeleteCard(int id);
}