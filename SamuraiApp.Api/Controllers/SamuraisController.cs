using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SamuraiApp.Domain;

namespace SamuraiApp.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class SamuraisController : ControllerBase
    {
        private readonly BusinessLogicData _bizLogic;

        public SamuraisController(BusinessLogicData bizLogic)
        {
            _bizLogic = bizLogic;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Samurai>>> GetSamurais()
        {
            var samurais = await _bizLogic.GetAllSamurais();
            return Ok(samurais);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Samurai>> GetSamurai(int id)
        {
            var samurai = await _bizLogic.GetSamuraiById(id);

            if (samurai == null)
            {
                return NotFound();
            }

            return Ok(samurai);
        }

        [HttpGet("said/{word}")]
        public async Task<ActionResult<List<Samurai>>> SamuraisWhoSaidAGivenWord(string word)
        {
            var samurais = await _bizLogic.GetSamuraisBySaidWord(word);
            if (samurais.Count == 0)
            {
                return NotFound("No samurai have ever said the given word");
            }
            return Ok(samurais);
        }

        [HttpPost]
        public async Task<ActionResult> Create(Samurai samurai)
        {
            await _bizLogic.AddSamurai(samurai);
            return CreatedAtAction("GetSamurais", new { id = samurai.Id }, samurai);
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, Samurai samurai)
        {
            if (id != samurai.Id)
            {
                return BadRequest(
                    "The ID passed in the URI differs from the ID passed in the request body");
            }
            await _bizLogic.UpdateWholeSamurai(samurai);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {   
            var samurai = await _bizLogic.GetSamuraiById(id);
            if (samurai == null)
            {
                return NotFound();
            }
            await _bizLogic.DeleteSamurai(samurai);

            return NoContent();
        }

        [HttpDelete("sproc/{id}")]
        public ActionResult DeleteQuotesForSamurai(int id)
        {
            var deletedQuotesCount = _bizLogic.DeleteAllQuotesBySamuraiId(id);

            return Ok($"{deletedQuotesCount} quotes deleted");
        }
    }
}
