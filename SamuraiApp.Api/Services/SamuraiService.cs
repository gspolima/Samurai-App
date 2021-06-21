using SamuraiApp.Api.Repositories;
using SamuraiApp.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SamuraiApp.Api.Services
{
    public class SamuraiService : ISamuraiService
    {
        private readonly ISamuraiRepository repo;

        public SamuraiService(ISamuraiRepository repo)
        {
            this.repo = repo;
        }

        public async Task<List<Samurai>> GetSamurais()
        {
            var samurais = await repo.GetAllSamuraisAsync();
            return samurais;
        }

        public async Task<Samurai> GetSamuraiById(int samuraiId)
        {
            var samurai = await repo.GetSamuraiAsync(samuraiId);
            return samurai;
        }

        public async Task<List<Samurai>> GetSamuraisByWordSpoken(string word)
        {
            var samurais = await repo.GetSamuraisByTerm(word);
            return samurais;
        }

        /* not working atm, to be continued...
        public List<Samurai> GetSamuraisWithNoQuote()
        {
            var quotes = repo.Quotes.ToArray();
            var samuraisWithQuotes = repo.Samurais
                .AsEnumerable()
                .GroupJoin(
                    quotes,
                    s => s.Id,
                    q => q.SamuraiId,
                    (s, q) => s)
                .Distinct(new SamuraiComparer())
                .ToList();

            return samuraisWithQuotes;
        } */

        public async Task<int> CreateNewSamurai(Samurai samurai)
        {
            var affectedRows = await repo.AddSamuraiAsync(samurai);
            return affectedRows;
        }

        public async Task<int> UpdateWholeSamuraiAsync(Samurai samurai)
        {
            var affectedRows = await repo.UpdateSamuraiAsync(samurai);
            return affectedRows;
        }

        public async Task<int> RemoveSamurai(Samurai samurai)
        {
            var affectedRows = await repo.DeleteSamuraiAsync(samurai);
            return affectedRows;
        }

        public int DeleteQuotesBySamuraiId(int samuraiId)
        {
            var affectedRows = repo.DeleteQuotesForSamurai(samuraiId);
            return affectedRows;
        }
    }
}