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
            GetSamurais("Before Add:");
            AddSamurais("Paul");
            GetSamurais("After Add:");
            Console.Write("Press Any Key...");
            Console.ReadKey();
        }

        private static void AddSamurais(params string[] samurais)
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
    }
}
