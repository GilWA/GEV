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
using X.PagedList;

namespace GestionEvenements.Controllers
{
    [Authorize]
    [TypeFilter(typeof(LogExceptionFilter))]
    public class EvenementsController : Controller
    {
        private readonly RepoEvenements _repo;

        public EvenementsController(RepoEvenements repo)
        {
            _repo = repo;
        }

        // GET: Evenement/Inscription/idParticipant
        public async Task<IActionResult> Inscription(int id, string texteRecherche)
        {
            var repoPa = new RepoParticipants(_repo.Context);
            var participant = await repoPa.LireAsync(id);
            if (participant == null)
            {
                return NotFound();
            }

            ViewBag.PaId = id;
            ViewBag.Participant = participant.ToString();
            if (string.IsNullOrEmpty(texteRecherche))
            {
                return View();
            }
            ViewBag.Filtre = texteRecherche.Trim();
            List<Evenement> evenements = _repo.Liste(ViewBag.Filtre);
            int n = evenements.Count;
            ViewBag.Resultat = n == 0 ? "Aucune correspondance" : $"{n} événements trouvés";
            return View(evenements);
        }

        [AllowAnonymous]
        public async Task<ViewResult> Index(string sensTri, int? page)
        {
            int nbrLignes = 3;
            if (string.IsNullOrEmpty(sensTri)) sensTri = "titre";
            ViewBag.SensTri = sensTri;
            ViewBag.TriTitre = sensTri == "titre" ? "titre_desc" : "titre";
            ViewBag.TriAnnee = sensTri == "annee" ? "annee_desc" : "annee";

            int numPage = (page ?? 1);
            return View(await _repo.EvenementsTries(sensTri).ToPagedListAsync(numPage, nbrLignes));
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evenement = await _repo.LireAsync(id.Value);
            if (evenement == null)
            {
                return NotFound();
            }

            return View(evenement);
        }

        public IActionResult Create()
        {
            ViewBag.TypeEvenementId = new SelectList(_repo.TypesEvenementAvecValeurVide, "TypeEvenementId", "Libelle");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EvenementId,TypeEvenementId,Annee,DateEvenement,Titre,Description,Url,AdresseId")] Evenement evenement)
        {
            if (ModelState.IsValid)
            {
                await _repo.Creer(evenement);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.TypeEvenementId = new SelectList(_repo.TypesEvenementAvecValeurVide, "TypeEvenementId", "Libelle");

            return View(evenement);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evenement = await _repo.LireAsync(id.Value);
            if (evenement == null)
            {
                return NotFound();
            }
            ViewBag.TypeEvenementId = new SelectList(_repo.TypesEvenementAvecValeurVide, "TypeEvenementId", "Libelle");
            return View(evenement);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EvenementId,TypeEvenementId,Annee,DateEvenement,Titre,Description,Url,AdresseId")] Evenement evenement)
        {
            if (id != evenement.EvenementId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _repo.Modifier(evenement);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_repo.Existe(evenement.EvenementId))
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

            ViewBag.TypeEvenementId = new SelectList(_repo.TypesEvenementAvecValeurVide, "TypeEvenementId", "Libelle");
            return View(evenement);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evenement = await _repo.LireAsync(id.Value);
            if (evenement == null)
            {
                return NotFound();
            }

            return View(evenement);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var evenement = await _repo.LireAsync(id);
            await _repo.Supprimer(evenement);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [ActionName("ra")]
        public async Task<IActionResult> RetirerAdresse(int id)
        {
            var evenement = await _repo.LireAsync(id);
            if (evenement == null)
            {
                return NotFound();
            }

            await _repo.RetirerAdresse(id);
            return RedirectToAction(nameof(Details), new { id = id });
        }
    }
}
