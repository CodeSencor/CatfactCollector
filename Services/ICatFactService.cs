using CatfactCollector.Models;

namespace CatfactCollector.Services;

public interface ICatFactService
{
    Task<CatFact?> GetRandomCatFactAsync();
}

