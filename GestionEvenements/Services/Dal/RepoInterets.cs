using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using GestionEvenements.Services.App;
using GestionEvenements.Services.Sessions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Modele;

namespace GestionEvenements.Services.Dal
{
    public class RepoInterets : RepoBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        public RepoInterets(GevDbContext context, UserManager<IdentityUser> userManager) : base(context)
        {
            _userManager = userManager;
        }

        private string _userId;
        private string UserId(ClaimsPrincipal user)
        {
            if (string.IsNullOrEmpty(_userId))
            {
                if (Parametres.EnvironnementDev
                    && (user.Identity == null || !user.Identity.IsAuthenticated))
                {
                    _userId = Parametres.UserIdDev;
                }
                else
                {
                    _userId = _userManager.GetUserId(user);
                }
            }

            return _userId;
        }

        public async Task<int> EnregistrerListe(List<Interet> selection, ClaimsPrincipal user)
        {
            if (selection.Count == 0)
            {
                return 0;
            }

            string userId = UserId(user);
            foreach (Interet interet in selection)
            {
                interet.UserId = userId;
            }

            string sql = $"Delete From Interets Where UserId='{userId}'";
            await Context.Database.ExecuteSqlRawAsync(sql);
            await Context.Interets.AddRangeAsync(selection);
            return await Context.SaveChangesAsync();
        }

        public List<Interet> ListeEnregistree(ClaimsPrincipal user)
        {
            string userId = UserId(user);
            NoTracking();
            var interets = Context.Interets
                .AsQueryable()
                .Where(it => it.UserId == userId)
                .Include(it => it.Evenement)
                .ToList();
            // Les intérêts sont récrits. Pas de mise à jour. L'Id est seulement conservé pour pouvoir créer les lignes
            foreach (var interet in interets)
            {
                interet.InteretId = 0;
                interet.TitreEvenement = interet.Evenement.Titre;
            }
            return interets;
        }

    }
}
