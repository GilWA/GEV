using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Modele;

namespace GestionEvenements.Services.Dal
{
    public class RepoAdresses : RepoBase, IRepositoryAdresses
    {
        public RepoAdresses(GevDbContext context) : base(context)
        {
        }
        public async Task<int> Creer(Adresse Adresse)
        {
            return await Enregistrer(Adresse, EntityState.Added);
        }
        public bool Existe(int id)
        {
            NoTracking();
            return Context.Adresses.Any(e => e.AdresseId == id);
        }
        public async Task<Adresse> LireAsync(int id)
        {
            NoTracking();
            var Adresse = Context.Adresses.AsQueryable()
            .SingleOrDefaultAsync(it => it.AdresseId == id);
            return await Adresse;
        }
        public IAsyncEnumerable<Adresse> Liste()
        {
            NoTracking();
            return Context.Adresses.AsAsyncEnumerable();
        }
        public IQueryable<Adresse> ListeIncluding(params Expression<Func<Adresse, object>>[] includeProperties)
        {
            NoTracking();
            return includeProperties.Aggregate<Expression<Func<Adresse, object>>, IQueryable<Adresse>>(Context.Adresses, (current, includeProperty) => current.Include(includeProperty));
        }
        public async Task<int> Modifier(Adresse Adresse)
        {
            return await Enregistrer(Adresse, EntityState.Modified);
        }
        public async Task<int> Supprimer(Adresse Adresse)
        {
            return await Enregistrer(Adresse, EntityState.Deleted);
        }
    }
}
