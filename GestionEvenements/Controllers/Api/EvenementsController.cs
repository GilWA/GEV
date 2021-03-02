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
    public class EvenementsController : ControllerBase
    {
        private readonly GevDbContext _context;

        public EvenementsController(GevDbContext context)
        {
            _context = context;
        }

        // POST: api/evenements/ca/idev/idad
        [HttpPost]
        [Route("ca/{idev}/{idad}")]
        public async Task<int> AssocierAdresse(string idev, string idad)
        {
            if (!int.TryParse(idev, out var ide) || !int.TryParse(idad, out var ida))
            {
                throw new ArgumentException($"Ids idEv={idev}, idAd{idad} invalides dans api/Evenements/ca !");
            }

            var repo = new RepoEvenements(_context);
            return await repo.AssocierAdresse(ide, ida);
        }
        // POST: api/evenements/inscription/idev/idpa
        [HttpPost]
        [Route("inscription/{idev}/{idpa}")]
        public async Task<int> Inscription(string idev, string idpa)
        {
            if (!int.TryParse(idev, out var ide) || !int.TryParse(idpa, out var ida))
            {
                throw new ArgumentException($"Ids idEv={idev}, idAd{idpa} invalides dans api/Evenements/ca !");
            }

            var repo = new RepoEvenements(_context);
            return await repo.Inscription(ide, ida);
        }

    }
}
