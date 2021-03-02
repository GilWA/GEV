using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GestionEvenements.Services.Dal;
using Microsoft.AspNetCore.Authorization;
using Modele;

namespace GestionEvenements.Controllers
{
    [Authorize]
    public class ParticipantsController : Controller
    {
        private readonly RepoParticipants _repo;

        public ParticipantsController(RepoParticipants repo)
        {
            _repo = repo;
        }

        // GET: Participants
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            // PN System.Linq.Async
            return View(await _repo.Liste().ToListAsync());
        }

        // GET: Participants/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var participant = await _repo.LireAsync(id.Value);
            if (participant == null)
            {
                return NotFound();
            }

            return View(participant);
        }

        // GET: Participants/Create
        public IActionResult Create()
        {
            return View();
        }

     [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ParticipantId,Civilite,Nom,Prenom,AnneeNaissance,AnneeDeces,LieuNaissance,Activites,Url")] Participant participant)
        {
            if (ModelState.IsValid)
            {
                await _repo.Creer(participant);
                return RedirectToAction(nameof(Index));
            }
            return View(participant);
        }

        // GET: Participants/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var participant = await _repo.LireAsync(id.Value);
            if (participant == null)
            {
                return NotFound();
            }
            return View(participant);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ParticipantId,Civilite,Nom,Prenom,AnneeNaissance,AnneeDeces,LieuNaissance,Activites,Url")] Participant participant)
        {
            if (id != participant.ParticipantId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _repo.Modifier(participant);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_repo.Existe(participant.ParticipantId))
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
            return View(participant);
        }

        // GET: Participants/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var participant = await _repo.LireAsync(id.Value);
            if (participant == null)
            {
                return NotFound();
            }

            return View(participant);
        }

        // POST: Participants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var participant = await _repo.LireAsync(id);
            await _repo.Supprimer(participant);
            return RedirectToAction(nameof(Index));
        }

    }
}
