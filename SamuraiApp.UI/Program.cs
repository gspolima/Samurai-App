using Microsoft.EntityFrameworkCore;
using SamuraiApp.Data;
using SamuraiApp.Domain;
using System;
using System.Linq;

namespace SamuraiApp.UI
{
    public class Program
    {
        static SamuraiContext _context = new SamuraiContext();

        static void Main(string[] args)
        {
            _context.Database.EnsureCreated();
            //AddSamuraisByName("Kojima", "Shinzu", "Yamaha", "Hideo");
            //AddVariousEntities();
            //GetSamurais("After Add:");
            QueryFilters();
            QueryAggregators();
            Console.Write("Press Any Key...");
            Console.ReadKey();
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
            var samuraiCount = _context.Samurais
                .TagWith("ConsoleApp.Program.GetSamurais is asking for a count!")
                .Count();
            var samurais = _context.Samurais.ToList();
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
            var samurais = _context.Samurais
                .Where(s => s.Name.Contains("C"));

            // table has to be full-text indexed
            var samuraisEfFunctions = _context.Samurais
                .Where(s => EF.Functions.Contains(s.Name, "C%"));

            foreach (var samurai in samurais)
            {
                Console.WriteLine(samurai.Name);
            }
        }

        public static void QueryAggregators()
        {
            var samurai = _context.Samurais
                .FirstOrDefault(s => s.Name.StartsWith("K"));

            Console.WriteLine(samurai.Name);

            var lastSamuraiOrderedByName = _context.Samurais
                .OrderBy(s => s.Name)
                .LastOrDefault();

            Console.WriteLine(lastSamuraiOrderedByName.Name);

            var samuraiById = _context.Samurais.Find(44);

            Console.WriteLine($"Id {samuraiById.Id} :: {samuraiById.Name}");
        }
    }
}
