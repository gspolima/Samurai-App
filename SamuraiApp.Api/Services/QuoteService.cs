using SamuraiApp.Api.Repositories;
using SamuraiApp.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SamuraiApp.Api.Services
{
    public class QuoteService : IQuoteService
    {
        private readonly IQuoteRepository repo;

        public QuoteService(IQuoteRepository repo)
        {
            this.repo = repo;
        }

        public async Task<List<Quote>> GetQuotes(int samuraiId)
        {
            var quotes = await repo.GetAllQuotesAsync(samuraiId);
            return quotes;
        }

        public async Task<Quote> GetQuoteById(int samuraiId, int quoteId)
        {
            var quote = await repo
                .GetQuoteAsync(samuraiId, quoteId);

            return quote;
        }

        public async Task<int> CreateNewQuote(Quote quote)
        {
            var affectedRows = await repo.AddQuoteAsync(quote);
            return affectedRows;
        }

        public async Task<int> UpdateWholeQuote(Quote quote)
        {
            var affectedRows = await repo.UpdateQuote(quote);
            return affectedRows;
        }

        public async Task<int> RemoveQuote(Quote quote)
        {
            var affectedRows = await repo.DeleteQuote(quote);
            return affectedRows;
        }
    }
}
