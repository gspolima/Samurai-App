using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SamuraiApp.Data;
using SamuraiApp.Domain;

namespace SamuraiApp.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class SamuraisController : ControllerBase
    {
        private readonly SamuraiContext _context;

        public SamuraisController(SamuraiContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Samurai>> GetSamurais()
        {
            var samurais = _context.Samurais.ToList();

            return samurais;
        }

        [HttpGet("{id}")]
        public ActionResult<Samurai> GetSamuraiById(int id)
        {
            var samurai = _context.Samurais.Find(id);

            if (samurai == null)
            {
                return NotFound();
            }

            return samurai;
        }

        [HttpPost]
        public ActionResult CreateNewSamurai(Samurai samurai)
        {
            _context.Samurais.Add(samurai);
            _context.SaveChanges();

            return CreatedAtAction("GetSamurais", new { id = samurai.Id }, samurai);
        }


        [HttpPut]
        public ActionResult UpdateSamurai(int id, Samurai samurai)
        {
            if (id != samurai.Id)
            {
                return BadRequest();
            }

            _context.Entry(samurai).State = EntityState.Modified;
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteSamurai(int id)
        {
            var samurai = _context.Samurais.Find(id);
            
            if (samurai == null)
            {
                return NotFound();
            }

            _context.Samurais.Remove(samurai);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
