using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionEvenements.Services.App;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GestionEvenements.Services.Dal;
using Microsoft.AspNetCore.Authorization;
using Modele;

namespace GestionEvenements.Controllers
{
    [Authorize]
    [TypeFilter(typeof(LogExceptionFilter))]
    public class AdressesController : Controller
    {
        private readonly RepoAdresses _repo;

        public AdressesController(RepoAdresses repo)
        {
            _repo = repo;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _repo.Liste().ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adresse = await _repo.LireAsync(id.Value);
            if (adresse == null)
            {
                return NotFound();
            }

            return View(adresse);
        }

        // GET: Adresses/Create/EvenementId? avec nom du paramètre id prédéfini
        public async Task<IActionResult> Create(int? id)
        {
            if (id.HasValue)
            {
                Evenement evenement = null;
                var repo = new RepoEvenements(_repo.Context);

                evenement = await repo.LireAsync(id.Value);

                if (evenement == null)
                {
                    return NotFound();
                }

                ViewBag.EvenementId = evenement.EvenementId;
                ViewBag.TitreEvenement = evenement.Titre;
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AdresseId,Titre,Rue,CodePostal,Ville,Region,Pays,Notes,Latitude,Longitude")] Adresse adresse, int? id)
        {
            if (ModelState.IsValid)
            {
                await _repo.Creer(adresse);

                // Associer la nouvelle adresse à l'événement si on a son Id
                if (id.HasValue)
                {
                    var repo = new RepoEvenements(_repo.Context);

                    var evenement = await repo.LireAsync(id.Value);

                    if (evenement == null)
                    {
                        return NotFound();
                    }
                    evenement.AdresseId = adresse.AdresseId;
                    await repo.Modifier(evenement);
                    return RedirectToAction(nameof(Details), "Evenements", new { id = id.Value });
                }

                return RedirectToAction(nameof(Index));
            }
            return View(adresse);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adresse = await _repo.LireAsync(id.Value);
            if (adresse == null)
            {
                return NotFound();
            }
            return View(adresse);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AdresseId,Titre,Rue,CodePostal,Ville,Region,Pays,Notes,Latitude,Longitude")] Adresse adresse)
        {
            if (id != adresse.AdresseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _repo.Modifier(adresse);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_repo.Existe(adresse.AdresseId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(adresse);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adresse = await _repo.LireAsync(id.Value);
            if (adresse == null)
            {
                return NotFound();
            }

            return View(adresse);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var adresse = await _repo.LireAsync(id);
            await _repo.Supprimer(adresse);
            return RedirectToAction(nameof(Index));
        }
    }
}
