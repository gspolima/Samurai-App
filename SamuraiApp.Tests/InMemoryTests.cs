using Microsoft.EntityFrameworkCore;
using SamuraiApp.Domain;
using SamuraiApp.UI;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xunit;

namespace SamuraiApp.Tests
{
    public class InMemoryTests
    {
        [Fact]
        public void CanInsertSamurai()
        {
            var samurai = new Samurai() { Name = "Test" };

            var inMemoryDB = new InMemoryInstanceFactory();
            var context = inMemoryDB.CreateNewInstance("CanInsertSamurai");
            context.Samurais.Add(samurai);

            Assert.Equal(EntityState.Added, context.Entry(samurai).State);

            Debug.WriteLine($"[InMemory] {samurai.Id} before SaveChanges");
            context.SaveChanges();
            Debug.WriteLine($"[InMemory] {samurai.Id} after SaveChanges");
        }

        [Fact]
        public void CanAddSamuraisByName()
        {
            var inMemoryDB = new InMemoryInstanceFactory();
            var context = inMemoryDB.CreateNewInstance("CanAddSamuraisByName");
            var bizLogic = new BizLogicData(context);

            var actual = bizLogic.AddNewSamuraisByName(
                new string[] { "Okamoto", "Shiro", "Dokutai" });

            Assert.Equal(3, actual);
        }

        [Fact]
        public void CanAddSingleSamurai()
        {
            var inMemoryDB = new InMemoryInstanceFactory();
            var context = inMemoryDB.CreateNewInstance("CanAddSingleSamurai");
            var bizLogic = new BizLogicData(context);

            var actual = bizLogic.InsertNewSamurai(new Samurai());

            Assert.Equal(1, actual);
        }

        [Fact]
        public void CanInsertSamuraiWithQuotes()
        {
            var samurai = new Samurai()
            {
                Name = "Test",
                Quotes = new List<Quote>()
                {
                    new Quote() { Text = "test quote 1" },
                    new Quote() { Text = "test quote 2" },
                    new Quote() { Text = "test quote 3" },
                    new Quote() { Text = "test quote 4" },
                }
            };

            var inMemoryDB = new InMemoryInstanceFactory();
            var context = inMemoryDB.CreateNewInstance("CanInsertSamuraiWithQuotes");
            var bizData = new BizLogicData(context);

            bizData.InsertNewSamurai(samurai);
            var samuraisAdded = context.Samurais
                .Where(s => s.Id == samurai.Id).Count();
            var quotesAdded = context.Quotes
                .Where(q => q.SamuraiId == samurai.Id).Count();

            Assert.Equal(1, samuraisAdded);
            Assert.Equal(4, quotesAdded);
        }

        [Fact]
        public void CanGetSamuraisWithQuotes()
        {
            var samurai = new Samurai()
            {
                Name = "Test",
                Quotes = new List<Quote>()
                {
                    new Quote() { Text = "test quote 1" },
                    new Quote() { Text = "test quote 2" },
                }
            };

            var inMemoryDB = new InMemoryInstanceFactory();
            var context = inMemoryDB.CreateNewInstance("CanGetSamuraisWithQuotes");
            var bizLogic = new BizLogicData(context);
            bizLogic.InsertNewSamurai(samurai);

            var samuraiWithQuotes = bizLogic.GetSamuraiWithQuotes(samurai.Id);

            Assert.Equal(2, samuraiWithQuotes.Quotes.Count);
        }
    }
}
