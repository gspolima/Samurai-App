using Microsoft.EntityFrameworkCore;
using SamuraiApp.Api;
using SamuraiApp.Data;
using SamuraiApp.Domain;
using System.Linq;
using Xunit;

namespace SamuraiApp.Tests
{
    public class IntegrationTests
    {
        [Fact]
        public void BizDataGetSamuraiReturnsSamurai()
        {
            var inMemoryDB = new InMemoryInstanceFactory();
            using var context = inMemoryDB.CreateNewInstance("BizDataGetSamuraiReturnsSamurai");

            var samurai = new Samurai() { Name = "test" };
            context.Samurais.Add(samurai);
            context.SaveChanges();

            var bizLogic = new BusinessLogicData(context);
            var samuraiRetrieved = bizLogic.GetSamuraiById(samurai.Id);

            Assert.Equal(samurai.Id, samuraiRetrieved.Result.Id);
        }

        [Fact]
        public void ContextIsNotTrackingAnyObjects()
        {
            var optionsBuilder = new DbContextOptionsBuilder<SamuraiContext>();
            optionsBuilder.UseInMemoryDatabase("ContextIsNotTrackingAnyObjects")
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            SeedOneSamurai(optionsBuilder.Options);

            using var context = new SamuraiContext(optionsBuilder.Options);
            context.Samurais.ToList();
            Assert.Empty(context.ChangeTracker.Entries());
        }

        private int SeedOneSamurai(DbContextOptions<SamuraiContext> options)
        {
            using (var seedContext = new SamuraiContext(options))
            {
                var samurai = new Samurai();
                seedContext.Samurais.Add(samurai);
                seedContext.SaveChanges();
                return samurai.Id;
            }
        }
    }
}