using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SamuraiApp.Data;
using SamuraiApp.Domain;

namespace SamuraiApp.Api
{
    public class BusinessLogicData
    {
        private readonly SamuraiContext _context;

        public BusinessLogicData(SamuraiContext context)
        {
            _context = context;
        }

        public async Task<List<Samurai>> GetAllSamurais()
        {
            var samurais = await _context.Samurais.ToListAsync();
            return samurais;
        }

        public async Task<Samurai> GetSamuraiById(int samuraiId)
        {
            var samurai = await _context.Samurais.FindAsync(samuraiId);
            return samurai;
        }

        public async Task<List<Samurai>> GetSamuraisBySaidWord(string word)
        {
            var samurais = await _context.Samurais
                .FromSqlInterpolated($"EXEC SamuraisWhoSaidAWord {word}")
                .ToListAsync();

            return samurais;
        }

        public async Task AddSamurai(Samurai samurai)
        {
            await _context.Samurais.AddAsync(samurai);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateWholeSamurai(Samurai samurai)
        {
            _context.Entry(samurai).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        
        public async Task DeleteSamurai(Samurai samurai)
        {
            _context.Samurais.Remove(samurai);
            await _context.SaveChangesAsync();
        }

        public int DeleteAllQuotesBySamuraiId(int id)
        {
            var affectedRows = _context.Database
                .ExecuteSqlInterpolated($"EXEC DeleteQuotesForSamurai {id}");
            return affectedRows;
        }
    }
}