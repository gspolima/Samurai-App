using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SamuraiApp.Data;
using SamuraiApp.Domain;

namespace SamuraiApp.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuotesController : ControllerBase
    {
        private readonly SamuraiContext _context;

        public QuotesController(SamuraiContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<List<Quote>> GetQuotes()
        {
            var quotes = _context.Quotes.ToList();
            
            return Ok(quotes);
        }


        [HttpGet("{id}")]
        public ActionResult<Quote> GetQuoteById(int id)
        {
            var quote = _context.Quotes.Find(id);

            if (quote == null)
            {
                return NotFound();
            }

            return Ok(quote);
        }

        [HttpGet("samurai/{id}")]
        public ActionResult<List<Quote>> QuotesBySamurai(int id)
        {
            var quotes = _context.Quotes
                .Where(q => q.SamuraiId == id)
                .ToList();

            if (quotes.Count == 0)
            {
                return NotFound("There are no quotes for this samurai");
            }

            return Ok(quotes);
        }

        [HttpGet("samurai/noquote")]
        public ActionResult SamuraisWithNoQuote()
        {
            var quotes = _context.Quotes.ToList();
            var samuraisWithQuotes = _context.Samurais
                .AsEnumerable()
                .Join(
                    quotes,
                    s => s.Id,
                    q => q.SamuraiId,
                    (s, q) => new { SamuraiId = s.Id })
                .Count();

            var samurais = _context.Samurais.Count();
            var samuraisWithNoQuotes = samurais - samuraisWithQuotes;

            return Ok($"{samuraisWithNoQuotes} Samurais have no recorded quotes");
        }

        [HttpPost]
        public ActionResult CreateNewQuote(Quote quote)
        {
            _context.Quotes.Add(quote);
            _context.SaveChanges();

            return CreatedAtAction("GetQuoteById", new { id = quote.Id }, quote );
        }

        [HttpPut("{id}")]
        public ActionResult UpdateQuoteText(int id, Quote quote)
        {
            if (id != quote.Id)
            {
                return BadRequest(
                    "The ID passed in the URI differs from the ID passed in the request body");
            }
            _context.Entry(quote).State = EntityState.Modified;
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteQuote(int id)
        {
            var quote = _context.Quotes.Find(id);

            if (quote == null)
            {
                return NotFound();
            }

            _context.Quotes.Remove(quote);
            _context.SaveChanges();

            return NoContent();
        }
    }
}