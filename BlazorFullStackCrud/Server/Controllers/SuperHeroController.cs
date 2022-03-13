using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorFullStackCrud.Server.Controllers
{
    [Route("api/super-hero")]
    [ApiController]
    public class SuperHeroController : ControllerBase
    {
        private readonly DataContext _context;

        public SuperHeroController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<SuperHero>>> GetSuperHeroes()
        {
            var heroes = await _context.SuperHero.Include(h => h.Comic).ToListAsync();
            return Ok(heroes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SuperHero>> GetSuperHero(int id)
        {
            var hero = await _context.SuperHero.Include(h => h.Comic).FirstOrDefaultAsync(h => h.Id == id);
            if (hero is null)
            {
                return NotFound("Hero not found");
            }

            return Ok(hero);
        }
        
        [HttpGet("comic")]
        public async Task<ActionResult<List<Comic>>> GetComics()
        {
            var comics = await _context.Comic.ToListAsync();
            return Ok(comics);
        }

        [HttpPost]
        public async Task<ActionResult<List<SuperHero>>> CreateSuperHero(SuperHero hero)
        {
            hero.Comic = null;
            _context.SuperHero.Add(hero);
            await _context.SaveChangesAsync();
            return Ok(await GetDbHeroes());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<List<SuperHero>>> UpdateSuperHero(int id, SuperHero hero)
        {
            var dbHero = await _context.SuperHero.Include(h => h.Comic).FirstOrDefaultAsync(h => h.Id == id);
            if (dbHero is null)
            {
                return NotFound("Hero not found");
            }

            dbHero.FirstName = hero.FirstName;
            dbHero.LastName = hero.LastName;
            dbHero.HeroName = hero.HeroName;
            dbHero.ComicId = hero.ComicId;

            await _context.SaveChangesAsync();
            return Ok(await GetDbHeroes());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<SuperHero>>> DeleteSuperHero(int id)
        {
            var dbHero = await _context.SuperHero.Include(h => h.Comic).FirstOrDefaultAsync(h => h.Id == id);
            if (dbHero is null)
            {
                return NotFound("Hero not found");
            }

            _context.SuperHero.Remove(dbHero);
            await _context.SaveChangesAsync();
            return Ok(await GetDbHeroes());
        }

        private async Task<List<SuperHero>> GetDbHeroes()
        {
            return await _context.SuperHero.Include(h => h.Comic).ToListAsync();
        }
    }
}
