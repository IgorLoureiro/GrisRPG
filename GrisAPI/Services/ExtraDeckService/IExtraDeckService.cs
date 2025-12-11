using GrisAPI.DTOs;

namespace GrisAPI.Services.ExtraDeckService;

public interface IExtraDeckService
{
    Task<ExtraDeckDto?> GetExtraDeckById(int id);

    Task<bool> UpdateExtraDeck(ExtraDeckDto extraDeck);
}