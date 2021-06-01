using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
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

            if (samurais == null)
                return NotFound();

            return samurais;
        }
    }
}
