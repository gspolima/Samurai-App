using SamuraiApp.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SamuraiApp.Api.Repositories
{
    public interface IQuoteRepository
    {
        Task<List<Quote>> GetAllQuotesAsync(int samuraiId);
        Task<Quote> GetQuoteAsync(int samuraiId, int quoteId);
        Task<int> AddQuoteAsync(Quote quote);
        Task<int> UpdateQuote(Quote quote);
        Task<int> DeleteQuote(Quote quote);
    }
}
