using SamuraiApp.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SamuraiApp.Api.Repositories
{
    public interface ISamuraiRepository
    {
        Task<List<Samurai>> GetAllSamuraisAsync();
        Task<Samurai> GetSamuraiAsync(int samuraiId);
        Task<IEnumerable<Samurai>> GetThreeSamuraisWithHorseAsync();
        Task<List<Samurai>> GetSamuraisByTerm(string word);
        Task<int> AddSamuraiAsync(Samurai samurai);
        Task<int> UpdateSamuraiAsync(Samurai samurai);
        Task<int> DeleteSamuraiAsync(Samurai samurai);
        int DeleteQuotesForSamurai(int samuraiId);
    }
}
