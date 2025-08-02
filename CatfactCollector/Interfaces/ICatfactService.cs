namespace CatfactCollector.Interfaces;

public interface ICatfactService
{ 
    Task<string> GetCatfactAsync();
}