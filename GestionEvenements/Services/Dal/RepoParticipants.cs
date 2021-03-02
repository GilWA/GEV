using Modele;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace GestionEvenements.Services.Dal
{
    public class RepoParticipants : RepoBase, IRepositoryParticipants
    {
        public RepoParticipants(GevDbContext context) : base(context)
        {
     
        }

        public async Task<int> Creer(Participant participant)
        {
            return await Enregistrer(participant, EntityState.Added);
        }

        public bool Existe(int id)
        {
            NoTracking();
            return Context.Participants.Any(e => e.ParticipantId == id);
        }

        public async Task<Participant> LireAsync(int id)
        {
            NoTracking();
            var participant = Context.Participants
                .Include(it => it.Participations)
                .ThenInclude(it => it.Evenement)
                .ThenInclude(it => it.TypeEvenement)
                .SingleOrDefaultAsync(it => it.ParticipantId == id);
            return await participant;
        }

        public IAsyncEnumerable<Participant> Liste()
        {
            NoTracking();
            return Context.Participants.AsAsyncEnumerable();
        }

        public IQueryable<Participant> ListeIncluding(params Expression<Func<Participant, object>>[] includeProperties)
        {
            NoTracking();
            return includeProperties.Aggregate<Expression<Func<Participant, object>>, IQueryable<Participant>>(Context.Participants, (current, includeProperty) => current.Include(includeProperty));
        }

        public async Task<int> Modifier(Participant participant)
        {
            return await Enregistrer(participant, EntityState.Modified);
        }

        public async Task<int> Supprimer(Participant participant)
        {
            return await Enregistrer(participant, EntityState.Deleted);
        }
    }
}
