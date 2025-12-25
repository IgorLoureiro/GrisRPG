using GrisAPI.DTOs;
using GrisAPI.Models;
using GrisAPI.Repositories.CardRepository;
using Microsoft.EntityFrameworkCore;

namespace GrisAPI.Services.CardService;

public sealed class CardService(ICardRepository cardRepository) : ICardService
{
    public async Task<CardFilterResponse> GetFilteredCards(CardFilterRequest filterRequest)
    {
        var filteredCards = await cardRepository.GetFilteredCards(filterRequest)
            .Select(card => new CardDto(card))
            .ToListAsync();

        var maxNumberOfPages = Convert.ToInt32(Math.Ceiling((double)filteredCards.Count / filterRequest.Quantity));
        
        return new CardFilterResponse
        {
            MaxNumberOfPages = maxNumberOfPages, 
            Cards = filteredCards
        };
    }
    
    public async Task<List<CardDto>> GetCardsById(List<int> ids)
    {
        return await cardRepository.GetCardsById(ids)
            .Select(card => new CardDto(card))
            .ToListAsync();
    }

    public async Task<Card?> GetCardById(int id)
    {
        return await cardRepository.GetCardById(id);
    }

    public async Task<Card?> AddCard(CardDto? cardDto)
    {
        if (cardDto is null)
            return null;

        var cardModel = new Card
        {
            Symbol = cardDto.Symbol,
            Manifestation = cardDto.Manifestation,
            Name = cardDto.Name,
            Description = cardDto.Description,
        };
        
        return await cardRepository.AddCard(cardModel);
    }

    public async Task<bool> UpdateCard(CardDto? cardDto)
    {
        if (cardDto is null)
            return false;
        
        var cardModel = await cardRepository.GetCardById(cardDto.Id);

        if (cardModel is null)
            return false;
        
        cardModel.Symbol = cardDto.Symbol;
        cardModel.Manifestation = cardDto.Manifestation;
        cardModel.Name = cardDto.Name;
        cardModel.Description = cardDto.Description;
        
        await cardRepository.UpdateCard(cardModel);
        return true;
    }

    public async Task<bool> DeleteCard(int id)
    {
        var cardModel = await cardRepository.GetCardById(id);

        if (cardModel is null)
            return false;
        
        await cardRepository.DeleteCard(cardModel);
        return true;
    }
}