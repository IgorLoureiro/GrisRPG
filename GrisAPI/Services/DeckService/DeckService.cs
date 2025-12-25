using GrisAPI.DTOs;
using GrisAPI.Models;
using GrisAPI.Repositories.CardRepository;
using GrisAPI.Repositories.CreatureRepository;
using GrisAPI.Repositories.DeckRepository;
using GrisAPI.Repositories.JokerRepository;
using Microsoft.EntityFrameworkCore;

namespace GrisAPI.Services.DeckService;

public sealed class DeckService(
    IDeckRepository deckRepository,
    ICardRepository cardRepository,
    IJokerRepository jokerRepository,
    ICreatureRepository creatureRepository) 
    : IDeckService
{
    public async Task<DeckDto?> GetDeckById(int id)
    {
        var deck = await deckRepository.GetDeckById(id);
        return deck is null? null : new DeckDto(deck);
    }

    public async Task<DeckDto?> AddDeck(DeckDto deck, int creatureId)
    {
        var creature = await creatureRepository.GetCreatureByIdAsync(creatureId);
        if (creature is null)
            return null;
        
        var deckModel = new Deck()
        {
            Name = deck.Name,
            Creatures = new List<Creature>()
            {
                creature
            }
        };

        var deckDto = await deckRepository.AddDeck(deckModel);
        return new DeckDto(deckDto);
    }
    
    public async Task<bool> UpdateDeck(DeckDto deck)
    {
        var deckModel = await deckRepository.GetDeckById(deck.Id);
        if(deckModel is null)
            return false;
        
        var cardsIdArray = deck.Cards.Select(x => x.Id).ToArray();
        var jokersIdArray = deck.Jokers.Select(x => x.Id).ToArray();
        
        var cards = await cardRepository.GetCardsById(cardsIdArray).ToListAsync();
        var jokers = await jokerRepository.GetJokersById(jokersIdArray);

        deckModel.Cards = cards;
        deckModel.Jokers = jokers;

        await deckRepository.UpdateDeck(deckModel);
        return true;
    }

    public async Task<bool> DeleteDeck(int id)
    {
        var deckModel = await deckRepository.GetDeckById(id);
        if(deckModel is null)
            return false;
        
        await deckRepository.DeleteDeck(deckModel);
        return true;
    }
}