using System.Linq;
using Microsoft.EntityFrameworkCore;
using SamuraiApp.Data;
using SamuraiApp.Domain;

namespace SamuraiApp.UI
{
    public class BizLogicData
    {
        private readonly SamuraiContext _context;

        public BizLogicData()
        {
            _context = new SamuraiContext();
        }

        public BizLogicData(SamuraiContext context)
        {
            _context = context;
        }

        public int AddNewSamuraisByName(params string[] names)
        {
            foreach (var name in names)
            {
                var samurai = new Samurai() { Name = name };
                _context.Samurais.Add(samurai);
            }
            var affectedRows = _context.SaveChanges();
            return affectedRows;
        }

        public int InsertNewSamurai(Samurai samurai)
        {
            _context.Samurais.Add(samurai);
            var affectedRows = _context.SaveChanges();
            return affectedRows;
        }

        public Samurai GetSamuraiWithQuotes(int id)
        {
            var samurai = _context.Samurais
                .Where(s => s.Id == id)
                .Include(s => s.Quotes).SingleOrDefault();
            return samurai;
        }
    }
}