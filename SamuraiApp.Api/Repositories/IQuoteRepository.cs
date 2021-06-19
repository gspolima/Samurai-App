using SamuraiApp.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SamuraiApp.Api.Services
{
    interface IQuoteRepository
    {
        Task<List<Quote>> GellAllQuotes(int samuraiId);
        Task<Quote> GetQuote(int id);


    }
}
