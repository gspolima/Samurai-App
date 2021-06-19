using Microsoft.EntityFrameworkCore;
using SamuraiApp.Data;
using SamuraiApp.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SamuraiApp.Api.Repositories
{
    public class SamuraiRepository : ISamuraiRepository
    {
        private readonly SamuraiContext context;

        public SamuraiRepository(SamuraiContext context)
        {
            this.context = context;
        }

        public async Task<List<Samurai>> GetAllSamuraisAsync()
        {
            var samurais = await context.Samurais.ToListAsync();
            return samurais;
        }

        public async Task<Samurai> GetSamuraiAsync(int samuraiId)
        {
            var samurai = await context.Samurais.FindAsync(samuraiId);
            return samurai;
        }

        public async Task<List<Samurai>> GetSamuraisByTerm(string term)
        {
            var samurais = await context.Samurais
                .FromSqlInterpolated($"EXEC SamuraisWhoSaidAWord {term}")
                .ToListAsync();

            return samurais;
        }

        public async Task<int> AddSamuraiAsync(Samurai samurai)
        {
            await context.Samurais.AddAsync(samurai);
            return await context.SaveChangesAsync();
        }

        public async Task<int> UpdateSamuraiAsync(Samurai samurai)
        {
            context.Entry(samurai).State = EntityState.Modified;
            return await context.SaveChangesAsync();
        }

        public async Task<int> DeleteSamuraiAsync(Samurai samurai)
        {
            context.Samurais.Remove(samurai);
            return await context.SaveChangesAsync();
        }

        public int DeleteQuotesForSamurai(int samuraiId)
        {
            return context.Database
                .ExecuteSqlInterpolated(
                    $"EXEC DeleteQuotesForSamurai {samuraiId}");
        }
    }
}
