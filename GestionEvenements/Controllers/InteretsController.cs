using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GestionEvenements.Services.Dal;
using GestionEvenements.Services.Sessions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Modele;
namespace GestionEvenements.Controllers
{
    [Authorize]
    public class InteretsController : Controller
    {
        private SelectionInterets _selection;

        private SelectionInterets Selection => _selection ??= new SelectionInterets(HttpContext);

        private readonly RepoInterets _repo;
        public InteretsController(RepoInterets repo)
        {
            _repo = repo;
        }

        // GET: Interets
        public IActionResult Index(string message)
        {
            List<Interet> interets = Selection.Liste();
            ViewBag.Message = !string.IsNullOrEmpty(message) ? message : $"{interets.Count} intérêt(s) en cours";
            return View(interets);
        }

        public IActionResult ListeEnregistree()
        {
            var interets = _repo.ListeEnregistree(User);
            Selection.ActualiserInterets(interets);

            return RedirectToAction(nameof(Index), new { Message = $"Lecture de {interets.Count} intérêts enregistrés" });
        }

        public async Task<IActionResult> EnregistrerListe()
        {
            var interets = await _repo.EnregistrerListe(Selection.Liste(), User);
            return RedirectToAction(nameof(Index), new { Message = "Liste enregistrée" });
        }

        // GET: Interets/Create
        public async Task<IActionResult> Create()
        {
            await AfficherListeEvAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InteretId,UserId,EvenementId,Rating,Commentaires")] Interet interet)
        {
            // Proposer la mise à jour si l'intérêt existe déjà
            if (Selection.Existe(interet.EvenementId))
            {
                return RedirectToAction(nameof(Edit), new { id = interet.EvenementId });
            }

            // La colonne est obligatoire si la selection est enregistrée en base. Elle est renseignée dans ce cas à l'enregistrement.
            ModelState.Remove(nameof(Interet.UserId));
            if (ModelState.IsValid)
            {
                // Mémoriser le Titre de l'événement indépendamment de la Pté de nav Evenement
                interet.TitreEvenement = await TitreEvenementAsync(interet.EvenementId);
                Selection.Ajouter(interet);
                return RedirectToAction(nameof(Index));
            }
            await AfficherListeEvAsync();
            return View(interet);
        }

        // GET: Interets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (!Selection.Existe(id.Value))
            {
                return NotFound();
            }

            await AfficherListeEvAsync();
            return View(Selection.Lire(id.Value));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("InteretId,UserId,EvenementId,Rating,Commentaires")] Interet interet)
        {
            if (id != interet.EvenementId)
            {
                return NotFound();
            }
            ModelState.Remove(nameof(Interet.UserId));
            if (ModelState.IsValid)
            {
                interet.TitreEvenement = await TitreEvenementAsync(interet.EvenementId);
                Selection.Modifier(interet);
                return RedirectToAction(nameof(Index));
            }
            await AfficherListeEvAsync();
            return View(interet);
        }

        // GET: Interets/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var interet = Selection.Lire(id.Value);
            if (interet == null)
            {
                return NotFound();
            }

            return View(interet);
        }

        // POST: Interets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            Selection.Supprimer(id);
            return RedirectToAction(nameof(Index));
        }

        async Task<string> TitreEvenementAsync(int evId)
        {
            var repoEv = new RepoEvenements(_repo.Context);
            var ev = await repoEv.LireAsync(evId);
            return ev.Titre;
        }
        async Task AfficherListeEvAsync()
        {
            var repoEv = new RepoEvenements(_repo.Context);
            ViewBag.EvenementId = new SelectList(await repoEv.Liste().ToListAsync(), "EvenementId", "Titre");
        }
    }
}
