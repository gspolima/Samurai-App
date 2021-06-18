using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SamuraiApp.Api.Models;
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
                .Include(s => s.Quotes)
                .SingleOrDefault(s => s.Id == samuraiId);

            if (samurai == null)
                return NotFound("Samurai not found");

            var quoteFromDB = samurai.Quotes.SingleOrDefault(q => q.Id == id);

            if (quoteFromDB == null)
            {
                return NotFound();
            }

            var quote = new QuoteDto()
            {
                Id = quoteFromDB.Id,
                Text = quoteFromDB.Text,
                SamuraiId = quoteFromDB.SamuraiId
            };

            return Ok(quote);
        }

        [HttpGet("/noquote")]
        public ActionResult SamuraisWithNoQuote()
        {
            var quotes = context.Quotes.ToArray();
            var samuraisWithQuotes = context.Samurais
                .AsEnumerable()
                .Join(
                    quotes,
                    s => s.Id,
                    q => q.SamuraiId,
                    (s, q) => new { SamuraiId = s.Id })
                .Count();

            var samurais = context.Samurais.Count();
            var samuraisWithNoQuotes = samurais - samuraisWithQuotes;

            return Ok(
                $"{samuraisWithNoQuotes} Samurais have no recorded quotes");
        }

        [HttpPost]
        public ActionResult CreateNewQuote(int samuraiId, [FromBody] QuoteForCreationDto quote)
        {
            if (string.IsNullOrEmpty(quote.Text))
                return BadRequest($"Property [{nameof(quote.Text)}] must have a value");

            var samurai = context.Samurais.Find(samuraiId);

            if (samurai == null)
                return NotFound("Samurai not found");

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
        public ActionResult UpdateQuoteText(int id, Quote quote)
        {
            if (id != quote.Id)
            {
                return BadRequest(
                    "The ID passed in the URI differs from the ID passed in the request body");
            }

            if (quote.SamuraiIdHasValue == false)
            {
                context.Attach(quote);
                context.Entry(quote).Property(q => q.Text).IsModified = true;
                context.SaveChanges();
                return NoContent();
            }

            return BadRequest(
                "This resource does not allow change of the samurai who said a quote.");
        }

        [HttpPut("{id}/{newSamuraiId}")]
        public ActionResult UpdateSamuraiWhoSaidQuote(int id, int newSamuraiId)
        {
            var quote = context.Quotes.Find(id);
            context.Attach(quote);
            quote.SamuraiId = newSamuraiId;
            context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteQuote(int id)
        {
            var quote = context.Quotes.Find(id);

            if (quote == null)
            {
                return NotFound();
            }

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
                return NotFound("None of the quote IDs specified exist.");

            if (quoteIDs.Count > selectedQuotes.Count)
                return BadRequest("Not all quote IDs specified exist.");

            context.Quotes.RemoveRange(selectedQuotes);
            context.SaveChanges();

            return NoContent();
        }
    }
}