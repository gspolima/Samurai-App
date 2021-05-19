using Microsoft.EntityFrameworkCore;
using SamuraiApp.Data;
using SamuraiApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SamuraiApp.UI
{
    public class Program
    {
        static SamuraiContext _context = new SamuraiContext();
        static SamuraiContextNoTracking _contextNT = new SamuraiContextNoTracking();

        static void Main(string[] args)
        {
            _context.Database.EnsureCreated();
            //AddSamuraisByName("Kojima", "Shinzu", "Yamaha", "Hideo");
            //AddVariousEntities();
            //GetSamurais("After Add:");
            //QueryFilters();
            //QueryAggregators();
            //RetrieveMultipleSamuraisAndDelete();
            //QueryBattlesAndUpdate_Disconnected();
            //InsertNewSamuraiWithAQuote();
            //InsertNewSamuraiWithManyQuotes();
            //AddQuoteToExistingSamuraiWhileTracked();
            //AddQuoteToExistingSamuraiNotTracked(36);
            EagerLoadSamuraisWithQuotes();
            Console.Write("Press Any Key...");
            Console.Read();
        }

        private static void AddSamuraisByName(params string[] samurais)
        {
            foreach (var samurai in samurais)
            {
                var newSamurai = new Samurai() { Name = samurai };
                _context.Samurais.Add(newSamurai);
            }
            _context.SaveChanges();
        }

        private static void GetSamurais(string message)
        {
            var samuraiCount = _contextNT.Samurais
                .TagWith("ConsoleApp.Program.GetSamurais is asking for a count!")
                .Count();
            var samurais = _contextNT.Samurais.ToList();
            Console.WriteLine($"{message} {samuraiCount}");
            foreach (var samurai in samurais)
            {
                Console.WriteLine(samurai.Name);
            }
        }

        public static void AddVariousEntities()
        {
            _context.AddRange(
                new Samurai() { Name = "Caw" },
                new Battle() { Name = "Battle of Zharki" },
                new Samurai() { Name = "Nikolai" },
                new Battle() { Name = "Battle of Gatka" },
                new Samurai() { Name = "Ponk" },
                new Battle() { Name = "Battle of Midway" },
                new Samurai() { Name = "Giu" },
                new Battle() { Name = "Battle of Pearl Harbor" }
            );

            //_context.Samurais.AddRange(
            //    new Samurai() { Name = "Caw" },
            //    new Samurai() { Name = "Nikolai" },
            //    new Samurai() { Name = "Ponk" },
            //    new Samurai() { Name = "Giu" });

            //_context.Battles.AddRange(
            //    new Battle() { Name = "Battle of Zharki" },
            //    new Battle() { Name = "Battle of Gatka" },
            //    new Battle() { Name = "Battle of Midway" },
            //    new Battle() { Name = "Battle of Pearl Harbor" });

            _context.SaveChanges();
        }

        public static void QueryFilters()
        {
            var samurais = _contextNT.Samurais
                .Where(s => s.Name.Contains("C"));

            // table has to be full-text indexed
            var samuraisEfFunctions = _contextNT.Samurais
                .Where(s => EF.Functions.Contains(s.Name, "C%"));

            foreach (var samurai in samurais)
            {
                Console.WriteLine(samurai.Name);
            }
        }

        public static void QueryAggregators()
        {
            var samurai = _contextNT.Samurais
                .FirstOrDefault(s => s.Name.StartsWith("K"));

            Console.WriteLine(samurai.Name);

            var lastSamuraiOrderedByName = _contextNT.Samurais
                .OrderBy(s => s.Name)
                .LastOrDefault();

            Console.WriteLine(lastSamuraiOrderedByName.Name);

            var samuraiById = _contextNT.Samurais.Find(44);

            Console.WriteLine($"Id {samuraiById.Id} :: {samuraiById.Name}");
        }

        public static void RetrieveMultipleSamuraisAndDelete()
        {
            var idList = new List<int>() { 43, 51 };
            var samurais = _contextNT.Samurais
                // join is faster than where(contains())
                .AsEnumerable()
                .Join(
                    inner: idList,
                    outerKeySelector: s => s.Id,
                    innerKeySelector: il => il,
                    (s, il) => s);

            _context.RemoveRange(samurais);
            _context.SaveChanges();
        }

        public static void QueryBattlesAndUpdate_Disconnected()
        {
            List<Battle> disconnectedBattles;
            using (var context1 = new SamuraiContextNoTracking())
            {
                disconnectedBattles = context1.Battles.ToList();
            }

            disconnectedBattles.ForEach(b =>
                {
                    b.StartDate = new DateTimeOffset(1575, 06, 28, 7, 0, 0, new TimeSpan(9, 0, 0));
                    b.EndDate = new DateTimeOffset(1570, 07, 30, 10, 0, 0, new TimeSpan(9, 0, 0));
                });

            using (var context2 = new SamuraiContextNoTracking())
            {
                context2.UpdateRange(disconnectedBattles);
                context2.SaveChanges();
            }
        }

        public static void InsertNewSamuraiWithAQuote()
        {
            var newSamurai = new Samurai()
            {
                Name = "Kambei Shimada",
                Quotes = new List<Quote>()
                {
                    new Quote() { Text = "Now that I saved you, will you take me for dinner?" }
                }
            };

            _context.Samurais.Add(newSamurai);
            _context.SaveChanges();
        }

        public static void InsertNewSamuraiWithManyQuotes()
        {
            var newSamurai = new Samurai()
            {
                Name = "Kyuzo",
                Quotes = new List<Quote>()
                {
                    new Quote() { Text = "Watch out for my sharp sword!" },
                    new Quote() { Text = "I told you to watch out! Oh well..." }
                }
            };

            _context.Samurais.Add(newSamurai);
            _context.SaveChanges();
        }

        public static void AddQuoteToExistingSamuraiWhileTracked()
        {
            var samurai = _context.Samurais.Skip(2).FirstOrDefault();

            samurai.Quotes = new List<Quote>()
            {
                new Quote() { Text = "Isn't it strange how little we change" }
            };

            _context.Samurais.Update(samurai);
            _context.SaveChanges();
        }

        public static void AddQuoteToExistingSamuraiNotTracked(int samuraiId)
        {
            var samurai = _context.Samurais.Find(samuraiId);

            samurai.Quotes = new List<Quote>()
            {
                new Quote() { Text = "The attached stars are whispering quietly." }
            };

            // new using syntax with C# 8
            using var newContext = new SamuraiContext();
            newContext.Attach(samurai);
            newContext.SaveChanges();
        }

        public static void EagerLoadSamuraisWithQuotes()
        {
            var samuraisWithFilteredQuotes = _context.Samurais
                .Include(s => s.Quotes
                    .Where(q => q.Text
                        .Contains("change")))
                .ToList();

            var samuraisWithQuotesAndBattles = _context.Samurais
                .Include(s => s.Quotes)
                .Where(s => s.Quotes.Count > 0)
                .Include(s => s.Battles)
                .ToList();
        }
    }
}