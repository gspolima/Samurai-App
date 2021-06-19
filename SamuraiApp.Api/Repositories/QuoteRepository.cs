using Microsoft.EntityFrameworkCore;
using SamuraiApp.Data;
using SamuraiApp.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SamuraiApp.Api.Services
{
    public class QuoteRepository
    {
        private readonly SamuraiContext context;

        public QuoteRepository(SamuraiContext context)
        {
            this.context = context;
        }

        public async Task<List<Quote>> GetQuotesAsync(int samuraiId)
        {
            var quotes = await context.Quotes
                .Where(q => q.SamuraiId == samuraiId)
                .ToListAsync();

            return quotes;
        }

        public async Task<Quote> GetQuoteAsync(int samuraiId, int quoteId)
        {
            var quote = await context.Quotes
                .Where(q =>
                    q.SamuraiId == samuraiId &&
                    q.Id == quoteId)
                .FirstOrDefaultAsync();

            return quote;
        }

        public async Task<int> AddQuoteAsync(Quote quote)
        {
            await context.Quotes.AddAsync(quote);
            return await context.SaveChangesAsync();
        }

        public async Task<int> UpdateQuote(Quote quote)
        {
            context.Entry(quote).State = EntityState.Modified;
            return await context.SaveChangesAsync();
        }

        public async Task<int> DeleteQuote(Quote quote)
        {
            context.Quotes.Remove(quote);
            return await context.SaveChangesAsync();
        }
    }
}