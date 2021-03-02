using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionEvenements.Services.Dal;

namespace GestionEvenements.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdressesController : ControllerBase
    {
        private readonly GevDbContext _context;

        public AdressesController(GevDbContext context)
        {
            _context = context;
        }
        // GET: api/Adresses/ra/cpVille
        // Ex : https://localhost:44397/api/adresses/rad/redmond
        [Route("rad/{cpville}")]
        public IActionResult RechercheAdresses(string cpVille)
        {
            var req = int.TryParse(cpVille, out var cp) ?
                _context.Adresses.AsQueryable().Where(it => it.CodePostal.Contains(cp.ToString())) :
                _context.Adresses.AsQueryable().Where(it => it.Ville.Contains(cpVille));
            var result = req.Select(it => new { it.AdresseId, it.CodePostal, it.Ville }).ToList();
            return Ok(result);
        }
    }
}
