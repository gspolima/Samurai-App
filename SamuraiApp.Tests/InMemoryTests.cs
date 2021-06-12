using System;
using Xunit;
using Microsoft.EntityFrameworkCore;
using SamuraiApp.Domain;
using SamuraiApp.Data;
using System.Diagnostics;
using SamuraiApp.UI;
using System.Collections.Generic;
using System.Linq;

namespace SamuraiApp.Tests
{
    public class InMemoryTests
    {
        [Fact]
        public void CanInsertSamurai()
        {
            var samurai = new Samurai() { Name = "Test" };

            var context = CreateNewInMemoryInstance("CanInsertSamurai");
            context.Samurais.Add(samurai);

            Assert.Equal(EntityState.Added, context.Entry(samurai).State);

            Debug.WriteLine($"[InMemory] {samurai.Id} before SaveChanges");
            context.SaveChanges();
            Debug.WriteLine($"[InMemory] {samurai.Id} after SaveChanges");
        }

        [Fact]
        public void CanAddSamuraisByName()
        {
            var context = CreateNewInMemoryInstance("CanAddSamuraisByName");
            var bizLogic = new BizLogicData(context);

            var actual = bizLogic.AddNewSamuraisByName(
                new string[] { "Okamoto", "Shiro", "Dokutai" });

            Assert.Equal(3, actual);
        }

        [Fact]
        public void CanAddSingleSamurai()
        {
            var context = CreateNewInMemoryInstance("CanAddSingleSamurai");
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

            var context = CreateNewInMemoryInstance("CanInsertSamuraiWithQuotes");
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

            var context = CreateNewInMemoryInstance("CanGetSamuraisWithQuotes");
            var bizLogic = new BizLogicData(context);
            bizLogic.InsertNewSamurai(samurai);

            var samuraiWithQuotes = bizLogic.GetSamuraiWithQuotes(samurai.Id);

            Assert.Equal(2, samuraiWithQuotes.Quotes.Count);
        }

        private static SamuraiContext CreateNewInMemoryInstance(string instanceName)
        {
            var optionsBuilder = new DbContextOptionsBuilder();
            optionsBuilder.UseInMemoryDatabase(instanceName);
            return new SamuraiContext(optionsBuilder.Options);
        }
    }
}
