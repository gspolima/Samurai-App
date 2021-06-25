using Microsoft.AspNetCore.Mvc;
using SamuraiApp.Api.Services;
using SamuraiApp.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SamuraiApp.Api
{
    [Route("api/samurais/")]
    [ApiController]
    public class SamuraisController : ControllerBase
    {
        private readonly ISamuraiService service;

        public SamuraisController(ISamuraiService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Samurai>>> GetSamurais()
        {
            var samurais = await service.GetSamurais();
            return Ok(samurais);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Samurai>> GetSamuraiById(int id)
        {
            var samurai = await service.GetSamuraiById(id);

            if (samurai == null)
            {
                return NotFound();
            }

            return Ok(samurai);
        }

        [HttpGet("with/horse")]
        public async Task<ActionResult<int>> GetFirstThreeSamuraisWithHorse()
        {
            var count = await service.TopThreeSamuraisWithHorse();
            return Ok(count);
        }

        [HttpGet("noquote")]
        public ActionResult SamuraisWithNoQuote()
        {
            return NotFound("Resource temporarily removed...");
        }

        [HttpGet("said/{word}")]
        public async Task<ActionResult<List<Samurai>>> SamuraisWhoSaidAGivenWord(string word)
        {
            var samurais = await service.GetSamuraisByWordSpoken(word);
            if (samurais.Count == 0)
            {
                return NotFound("No samurai have ever said the given word");
            }
            return Ok(samurais);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Samurai samurai)
        {
            await service.CreateNewSamurai(samurai);
            return CreatedAtAction("GetSamurais", new { id = samurai.Id }, samurai);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Samurai samurai)
        {
            if (id != samurai.Id)
            {
                return BadRequest(
                    "The ID passed in the URI differs from the ID passed in the request body");
            }
            await service.UpdateWholeSamuraiAsync(samurai);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var samurai = await service.GetSamuraiById(id);
            if (samurai == null)
            {
                return NotFound();
            }
            await service.RemoveSamurai(samurai);

            return NoContent();
        }

        [HttpDelete("sproc/{id}")]
        public IActionResult DeleteQuotesForSamurai(int id)
        {
            var deletedQuotesCount = service.DeleteQuotesBySamuraiId(id);

            return Ok($"{deletedQuotesCount} quotes deleted for samurai ID:{id}");
        }
    }
}
