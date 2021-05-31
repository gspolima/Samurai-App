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
            //QuerySamuraiBattleStats();
            //QueryDataWithRawSql();
            //QueryRelatedDataWithRawSql();
            QueryDataWithInterpolatedString("Kambei Shimada");
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
            // var findOneSamurai = _context.SamuraiBattleStats.Find(55);
        }

        public static void QueryDataWithRawSql()
        {
            var samurais = _context.Samurais
                .FromSqlRaw("SELECT * FROM SAMURAIS")
                .ToList();
        }

        public static void QueryRelatedDataWithRawSql()
        {
            var samuraisWithQuotes = _context.Samurais
                .FromSqlRaw("SELECT ID, NAME FROM SAMURAIS")
                .Include(s => s.Quotes)
                .Include(s => s.Horse)
                .ToList();
        }

        public static void QueryDataWithInterpolatedString(string samuraiName)
        {

            /*
                Not using the interpolated version for
                raw SQL may expose the database to SQL Injections.
            */
            var samurai = _context.Samurais
                .FromSqlInterpolated($"SELECT * FROM SAMURAIS WHERE NAME = {samuraiName}")
                .ToList();
        }
    }
}