using SamuraiApp.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SamuraiApp.Api.Services
{
    public interface IQuoteService
    {
        Task<List<Quote>> GetQuotes(int samuraiId);
        Task<Quote> GetQuoteById(int samuraiId, int quoteId);
        Task<int> CreateNewQuote(Quote quote);
        Task<int> UpdateWholeQuote(Quote quote);
        Task<int> RemoveQuote(Quote quote);
    }
}
