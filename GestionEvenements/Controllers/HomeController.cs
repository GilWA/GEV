using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GestionEvenements.Services;
using GestionEvenements.Services.App;
using GestionEvenements.ViewModels;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Modele; // Sessions


namespace GestionEvenements.Controllers
{
   
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Tests(string message)
        {
            ViewBag.Message = message;
            //throw new ApplicationException("Erreur non gérée !");
            return View();
        }

        [Route("tp")]
        public IActionResult TP()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            // Un log est fait / défaut dans le ficher d'ev. Applications du système
            // Pour ajouter d'autres infos :
            var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            if (exceptionHandlerPathFeature?.Error is ApplicationException)
            {
                _logger.LogError($"Log par IActionResult Error : Erreur App {exceptionHandlerPathFeature?.Error.Message} dans {exceptionHandlerPathFeature?.Path }");
            }
            else
            {
                _logger.LogError($"Log par IActionResult Error : Erreur non gérée {exceptionHandlerPathFeature?.Error.Message} dans {exceptionHandlerPathFeature?.Path }");

            }

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
