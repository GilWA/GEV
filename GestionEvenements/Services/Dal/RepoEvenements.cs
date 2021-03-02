using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Modele;

namespace GestionEvenements.Services.Dal
{
    public class RepoEvenements : RepoBase, IRepositoryEvenements
    {
        public RepoEvenements(GevDbContext context) : base(context)
        {

        }

        public async Task<int> Creer(Evenement evenement)
        {
            return await Enregistrer(evenement, EntityState.Added);
        }

        public bool Existe(int id)
        {
            NoTracking();
            return Context.Evenements.Any(e => e.EvenementId == id);
        }

        public async Task<Evenement> LireAsync(int id)
        {
            NoTracking();
            var Evenement = Context.Evenements
                .Include(it => it.Adresse)
                .Include(it => it.TypeEvenement)
                .Include(it => it.Participations)
                .ThenInclude(it => it.Participant)
                .SingleOrDefaultAsync(it => it.EvenementId == id);
            return await Evenement;
        }

        public IAsyncEnumerable<Evenement> Liste()
        {
            NoTracking();
            return Context.Evenements
                .Include(it => it.TypeEvenement)
                .AsQueryable()
                .OrderBy(it => it.Annee)
                .AsAsyncEnumerable();
        }

        public List<Evenement> Liste(string filtre)
        {
            NoTracking();
            return Context.Evenements
                .AsQueryable()
                .Where(it => it.Titre.Contains(filtre))
                .OrderBy(it => it.Annee)
                .ToList();
        }

        public IQueryable<Evenement> ListeIncluding(params Expression<Func<Evenement, object>>[] includeProperties)
        {
            NoTracking();
            return includeProperties.Aggregate<Expression<Func<Evenement, object>>, IQueryable<Evenement>>(Context.Evenements, (current, includeProperty) => current.Include(includeProperty));
        }

        public async Task<int> Modifier(Evenement evenement)
        {
            return await Enregistrer(evenement, EntityState.Modified);
        }

        public async Task<int> Supprimer(Evenement evenement)
        {
            return await Enregistrer(evenement, EntityState.Deleted);
        }

        private static readonly TypeEvenement _typeEvNull = new TypeEvenement { TypeEvenementId = -1, Libelle = "(Type d'événement)" };

        private static List<TypeEvenement> _typesEvenementAvecValeurVide;
        public List<TypeEvenement> TypesEvenementAvecValeurVide
        {
            get
            {
                if (_typesEvenementAvecValeurVide == null)
                {
                    _typesEvenementAvecValeurVide =
                        Context.TypesEvenement.AsQueryable().OrderBy(it => it.Libelle).ToList();

                    _typesEvenementAvecValeurVide.Insert(0, _typeEvNull);
                }

                return _typesEvenementAvecValeurVide;
            }
        }

        public void RazTypesEvenement()
        {
            _typesEvenementAvecValeurVide = null;
        }

        public IQueryable<Evenement> EvenementsTries(string sensTri)
        {
            if (string.IsNullOrEmpty(sensTri)) sensTri = "titre";

            IQueryable<Evenement> evenements = Context.Evenements;

            switch (sensTri)
            {
                case "titre":
                    evenements = evenements.OrderBy(s => s.Titre);
                    break;
                case "titre_desc":
                    evenements = evenements.OrderByDescending(s => s.Titre);
                    break;
                case "annee":
                    evenements = evenements.OrderBy(s => s.Annee);
                    break;
                case "annee_desc":
                    evenements = evenements.OrderByDescending(s => s.Annee);
                    break;
                default:
                    evenements = evenements.OrderBy(s => s.Titre);
                    break;
            }

            return evenements.AsQueryable();
        }

        #region Gestion des associations (ide = idEvenement, ida = idAdresse, idp = idParticipant)

        public async Task<int> AssocierAdresse(int ide, int ida)
        {
            string sql = $"Update Evenements Set AdresseId={ida} Where EvenementId={ide}";
            return await Context.Database.ExecuteSqlRawAsync(sql);
        }
        public async Task<int> RetirerAdresse(int ide)
        {
            string sql = $"Update Evenements Set AdresseId=NULL Where EvenementId={ide}";
            return await Context.Database.ExecuteSqlRawAsync(sql);
        }

        public async Task<int> Inscription(int idev, int idpa)
        {
            string sql = $"Insert Into Participations (EvenementId, ParticipantId)  Values ({idev}, {idpa})";
            return await Context.Database.ExecuteSqlRawAsync(sql);
        }

        #endregion
    }
}
