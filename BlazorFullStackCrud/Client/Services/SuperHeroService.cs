using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;

namespace BlazorFullStackCrud.Client.Services
{
    public class SuperHeroService : ISuperHeroService
    {
        private readonly HttpClient _http;
        private readonly NavigationManager _navigationManager;

        public SuperHeroService(HttpClient http, NavigationManager navigationManager)
        {
            _http = http;
            _navigationManager = navigationManager;
        }

        public List<SuperHero> Heroes { get; set; } = new List<SuperHero>();
        public List<Comic> Comics { get; set; } = new List<Comic>();

        public async Task GetSuperHeroes()
        {
            var result = await _http.GetFromJsonAsync<List<SuperHero>>("api/super-hero");
            if (result is not null)
            {
                Heroes = result;
            }
        }

        public async Task<SuperHero> GetHero(int id)
        {
            var result = await _http.GetFromJsonAsync<SuperHero>($"api/super-hero/{id}");
            if (result is not null)
            {
                return result;
            }

            throw new Exception("Hero not found");
        }

        public async Task GetComics()
        {
            var result = await _http.GetFromJsonAsync<List<Comic>>("api/super-hero/comic");
            if (result is not null)
            {
                Comics = result;
            }
        }

        public async Task CreateSuperHero(SuperHero hero)
        {
            var result = await _http.PostAsJsonAsync("api/super-hero", hero);
            await SetHeroes(result);
        }

        public async Task UpdateSuperHero(int id, SuperHero hero)
        {
            var result = await _http.PutAsJsonAsync($"api/super-hero/{id}", hero);
            await SetHeroes(result);
        }

        public async Task DeleteSuperHero(int id)
        {
            var result = await _http.DeleteAsync($"api/super-hero/{id}");
            await SetHeroes(result);
        }

        private async Task SetHeroes(HttpResponseMessage result)
        {
            var response = await result.Content.ReadFromJsonAsync<List<SuperHero>>();
            Heroes = response ?? new List<SuperHero>();

            _navigationManager.NavigateTo("superheroes");
        }
    }
}
