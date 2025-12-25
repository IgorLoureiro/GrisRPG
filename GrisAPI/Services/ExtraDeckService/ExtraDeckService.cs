using GrisAPI.DTOs;
using GrisAPI.Repositories.CardRepository;
using GrisAPI.Repositories.ExtraDeckRepository;
using GrisAPI.Repositories.JokerRepository;
using Microsoft.EntityFrameworkCore;

namespace GrisAPI.Services.ExtraDeckService;

public sealed class ExtraDeckService(
    IExtraDeckRepository extraDeckRepository,
    ICardRepository cardRepository,
    IJokerRepository jokerRepository) 
    : IExtraDeckService
{
    public async Task<ExtraDeckDto?> GetExtraDeckById(int id)
    {
        var extraDeckModel = await extraDeckRepository.GetExtraDeckById(id);
        return extraDeckModel is null ? null : new ExtraDeckDto(extraDeckModel);
    }

    public async Task<bool> UpdateExtraDeck(ExtraDeckDto extraDeck)
    {
        var extraDeckModel = await extraDeckRepository.GetExtraDeckById(extraDeck.Id);
        if(extraDeckModel is null)
            return false;
        
        var cardsIdArray = extraDeck.Cards.Select(x => x.Id).ToArray();
        var jokersIdArray = extraDeck.Jokers.Select(x => x.Id).ToArray();
        
        var cards = await cardRepository.GetCardsById(cardsIdArray).ToListAsync();
        var jokers = await jokerRepository.GetJokersById(jokersIdArray);

        extraDeckModel.Cards = cards;
        extraDeckModel.Jokers = jokers;

        await extraDeckRepository.UpdateExtraDeck(extraDeckModel);
        return true;
    }
}