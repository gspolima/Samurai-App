using SamuraiApp.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SamuraiApp.Api.Services
{
    public interface ISamuraiService
    {
        Task<List<Samurai>> GetSamurais();
        Task<Samurai> GetSamuraiById(int samuraiId);
        Task<IEnumerable<Samurai>> TopThreeSamuraisWithHorse();
        Task<List<Samurai>> GetSamuraisByWordSpoken(string word);
        Task<int> CreateNewSamurai(Samurai samurai);
        Task<int> UpdateWholeSamuraiAsync(Samurai samurai);
        Task<int> RemoveSamurai(Samurai samurai);
        int DeleteQuotesBySamuraiId(int samuraiId);
    }
}
