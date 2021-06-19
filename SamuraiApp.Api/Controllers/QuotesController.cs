using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SamuraiApp.Api.Dtos;
using SamuraiApp.Data;
using SamuraiApp.Domain;
using System.Collections.Generic;
using System.Linq;

namespace SamuraiApp.Api
{
    [ApiController]
    [Route("api/samurai/{samuraiId}/quotes")]
    public class QuotesController : ControllerBase
    {
        private readonly SamuraiContext context;

        public QuotesController(SamuraiContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public ActionResult<List<Quote>> GetQuotes(int samuraiId)
        {
            var samurai = context.Samurais
                .Include(s => s.Quotes)
                .SingleOrDefault(s => s.Id == samuraiId);

            if (samurai == null)
                return NotFound("Samurai not found");

            var quotesToReturn = new List<QuoteDto>();

            foreach (var quote in samurai.Quotes)
            {
                quotesToReturn.Add(
                    new QuoteDto()
                    {
                        Id = quote.Id,
                        Text = quote.Text,
                        SamuraiId = quote.SamuraiId
                    });
            }

            return Ok(quotesToReturn);
        }


        [HttpGet("{id}", Name = "GetQuoteById")]
        public ActionResult<Quote> GetQuoteById(int samuraiId, int id)
        {
            var samurai = context.Samurais
                .Include(s => s.Quotes.Where(q => q.Id == id))
                .SingleOrDefault(s => s.Id == samuraiId);

            if (samurai == null)
                return NotFound("Samurai ID not found");

            if (samurai.Quotes.Count == 0)
                return NotFound("Quote ID not found");

            var quoteFromDB = samurai.Quotes
                .SingleOrDefault(q => q.Id == id);

            var quote = new QuoteDto()
            {
                Id = quoteFromDB.Id,
                Text = quoteFromDB.Text,
                SamuraiId = quoteFromDB.SamuraiId
            };

            return Ok(quote);
        }



        [HttpPost]
        public ActionResult CreateNewQuote(
            int samuraiId, [FromBody] QuoteForCreationDto quote)
        {
            if (string.IsNullOrEmpty(quote.Text))
                return BadRequest($"Property [{nameof(quote.Text)}] must have a value");

            var samurai = context.Samurais.Find(samuraiId);

            if (samurai == null)
                return NotFound("Samurai ID not found");

            var newQuote = new Quote()
            {
                Text = quote.Text,
                SamuraiId = samurai.Id
            };

            context.Quotes.Add(newQuote);
            context.SaveChanges();

            var quoteDto = new QuoteDto()
            {
                Id = newQuote.Id,
                Text = newQuote.Text,
                SamuraiId = newQuote.SamuraiId
            };

            return CreatedAtAction(
                "GetQuoteById",
                new { samuraiId = quoteDto.SamuraiId, id = quoteDto.Id },
                quoteDto);
        }

        [HttpPut("{id}")]
        public ActionResult UpdateWholeQuote(
            int samuraiId, int id, [FromBody] QuoteForUpdateDto quote)
        {
            var samurai = context.Samurais
                .Include(s => s.Quotes.Where(q => q.Id == id))
                .SingleOrDefault(s => s.Id == samuraiId);

            if (samurai == null)
                return NotFound("Samurai ID not found");

            if (samurai.Quotes.Count == 0)
                return NotFound("Quote ID not found");

            var quoteFromDB = samurai.Quotes
                .SingleOrDefault(q => q.Id == id);

            if (quote.Text == quoteFromDB.Text || quote.SamuraiId == quoteFromDB.SamuraiId)
            {
                return BadRequest(
                    $"[{nameof(quote.Text)}] and [{nameof(quote.SamuraiId)}] values can't be the same as the original ones.");
            }

            context.Attach(quoteFromDB);

            quoteFromDB.Text = quote.Text;
            quoteFromDB.SamuraiId = quote.SamuraiId;

            context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteQuote(int samuraiId, int id)
        {
            var samurai = context.Samurais
                .Include(q => q.Quotes.Where(q => q.Id == id))
                .SingleOrDefault(s => s.Id == samuraiId);

            if (samurai == null)
                return NotFound("Samurai ID not found");

            if (samurai.Quotes.Count == 0)
                return NotFound("Quote ID not found");

            var quote = samurai.Quotes
                .SingleOrDefault(q => q.Id == id);

            context.Quotes.Remove(quote);
            context.SaveChanges();

            return NoContent();
        }

        [HttpDelete]
        public ActionResult DeleteMultipleQuotes(List<int> quoteIDs)
        {
            var selectedQuotes = context.Quotes
                .AsEnumerable()
                .Join(
                    quoteIDs,
                    q => q.Id,
                    qi => qi,
                    (q, qi) => q)
                .ToList();

            if (selectedQuotes == null)
                return NotFound("None of the specified quotes exist.");

            if (quoteIDs.Count > selectedQuotes.Count)
                return BadRequest("Not all specified quotes exist.");

            context.Quotes.RemoveRange(selectedQuotes);
            context.SaveChanges();

            return NoContent();
        }
    }
}