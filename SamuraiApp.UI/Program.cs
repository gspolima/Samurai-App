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
            QuerySamuraiBattleStats();
            Console.Write("Press Any Key...");
            Console.Read();
        }

        public static void QuerySamuraiBattleStats()
        {
            var stats = _context.SamuraiBattleStats.ToList();
            var firstSamurai = _context.SamuraiBattleStats.FirstOrDefault();
            
            /*
                Although the compiler is happy, it will 
                throw an exception because Find() doesn't
                work with keyless entities.
            */
            //var findOneSamurai = _context.SamuraiBattleStats.Find(55);
        }
    }
}