using GrisAPI.DTOs;
using GrisAPI.Models;

namespace GrisAPI.Repositories.CardRepository;

public interface ICardRepository
{
    IQueryable<Card> GetFilteredCards(CardFilterRequest filterRequest);
    IQueryable<Card> GetCardsById(IEnumerable<int> ids);
    Task<Card?> GetCardById(int id);
    Task<Card> AddCard(Card card);
    Task UpdateCard(Card card);
    Task DeleteCard(Card card);
}