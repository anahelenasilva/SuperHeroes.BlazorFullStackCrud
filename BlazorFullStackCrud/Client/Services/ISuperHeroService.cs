namespace BlazorFullStackCrud.Client.Services
{
    public interface ISuperHeroService
    {
        List<SuperHero> Heroes { get; set; }
        List<Comic> Comics { get; set; }

        Task GetSuperHeroes();
        Task<SuperHero> GetHero(int id);
        Task GetComics();
        Task CreateSuperHero(SuperHero hero);
        Task UpdateSuperHero(int id, SuperHero hero);
        Task DeleteSuperHero(int id);
    }
}
