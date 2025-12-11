using GrisAPI.Models;

namespace GrisAPI.Repositories.ExtraDeckRepository;

public interface IExtraDeckRepository
{
    Task<ExtraDeck?> GetExtraDeckById(int id);

    Task UpdateExtraDeck(ExtraDeck extraDeck);
}