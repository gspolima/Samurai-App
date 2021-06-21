using Microsoft.AspNetCore.Mvc;
using SamuraiApp.Api.Dtos.Quotes;
using SamuraiApp.Api.Services;
using SamuraiApp.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SamuraiApp.Api
{
    [ApiController]
    [Route("api/samurai/{samuraiId}/quotes")]
    public class QuotesController : ControllerBase
    {
        private readonly IQuoteService quoteService;
        private readonly ISamuraiService samuraiService;

        public QuotesController(IQuoteService quoteService, ISamuraiService samuraiService)
        {
            this.quoteService = quoteService;
            this.samuraiService = samuraiService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Quote>>> GetQuotes(int samuraiId)
        {
            var quotes = await quoteService.GetQuotes(samuraiId);

            if (quotes.Count == 0)
                return NotFound($"No quotes for Samurai ID [{samuraiId}]");

            var quotesToReturn = new List<QuoteDto>();

            foreach (var quote in quotes)
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
        public async Task<ActionResult<Quote>> GetQuoteById(int samuraiId, int id)
        {
            var quote = await quoteService.GetQuoteById(samuraiId, id);

            if (quote == null)
                return NotFound("Quote ID not found");

            var quoteToReturn = new QuoteDto()
            {
                Id = quote.Id,
                Text = quote.Text,
                SamuraiId = quote.SamuraiId
            };

            return Ok(quoteToReturn);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewQuote(
            int samuraiId, [FromBody] QuoteForCreationDto quote)
        {
            if (string.IsNullOrEmpty(quote.Text))
                return BadRequest($"Property [{nameof(quote.Text)}] must have a value");

            var samurai = samuraiService.GetSamuraiById(samuraiId);

            if (samurai == null)
                return NotFound($"Samurai ID [{samuraiId}] not found");

            var newQuote = new Quote()
            {
                Text = quote.Text,
                SamuraiId = samuraiId
            };

            await quoteService.CreateNewQuote(newQuote);

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
        public async Task<IActionResult> UpdateWholeQuote(
            int samuraiId, int id, [FromBody] QuoteForUpdateDto quote)
        {
            var samurai = await samuraiService.GetSamuraiById(quote.SamuraiId);

            if (samurai == null)
            {
                return NotFound(
                    $"Samurai ID [{quote.SamuraiId}] passed in the body was not found");
            }

            var quoteFromDB = await quoteService.GetQuoteById(samuraiId, id);

            if (quoteFromDB == null)
                return NotFound("Quote ID not found");

            if (quote.Text == quoteFromDB.Text || quote.SamuraiId == quoteFromDB.SamuraiId)
            {
                return BadRequest(
                    $"[{nameof(quote.Text)}] and [{nameof(quote.SamuraiId)}] values can't be the same as the original ones.");
            }

            quoteFromDB.Text = quote.Text;
            quoteFromDB.SamuraiId = quote.SamuraiId;

            var updatedRows =
                await quoteService.UpdateWholeQuote(quoteFromDB);

            if (updatedRows == 1)
                return NoContent();
            else
                return StatusCode(500, "Could not update due to a server error");

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuote(int samuraiId, int id)
        {
            var quote = await quoteService.GetQuoteById(samuraiId, id);

            if (quote == null)
                return NotFound();

            var deletedRows = await quoteService.RemoveQuote(quote);

            if (deletedRows == 1)
                return NoContent();
            else
                return StatusCode(500, "Could not update due to a server error");
        }
    }
}