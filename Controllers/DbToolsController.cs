using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionEvenements.Services.Dal;
using Microsoft.AspNetCore.Authorization;

namespace GestionEvenements.Controllers
{
    [Authorize]
    public class DbToolsController : Controller
    {
        private GevDbContext _context;
        public DbToolsController(GevDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(string message)
        {
            int nbrEvenements = _context.Evenements.Count();
            int nbrParticipants = _context.Participants.Count();
            int nbrParticipations = _context.Participations.Count();
            int nbrUtilisateurs = _context.Users.Count();

            ViewBag.NbrEvenements = nbrEvenements;
            ViewBag.NbrParticipants = nbrParticipants;
            ViewBag.NbrParticipations = nbrParticipations;
            ViewBag.NbrUtilisateurs = nbrUtilisateurs;
            ViewBag.Message = message;

            return View();
        }
        public IActionResult Migration()
        {
            string info = _context.Migrer();
            return RedirectToAction(nameof(Index), new {message= info});
        }
    }
}
