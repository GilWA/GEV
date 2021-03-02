using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Modele;

namespace GestionEvenements.Services.Dal
{
    public interface IRepository<T>
    {
        IAsyncEnumerable<T> Liste();
        IQueryable<T> ListeIncluding(params Expression<Func<T, object>>[] includeProperties);
        Task<T> LireAsync(int id);
        bool Existe(int id);
        Task<int> Creer(T contact);
        Task<int> Modifier(T entite);
        Task<int> Supprimer(T entite);
    }
    public interface IRepositoryParticipants: IRepository<Participant>
    {
    }
    public interface IRepositoryEvenements : IRepository<Evenement>
    {
    }
    public interface IRepositoryAdresses : IRepository<Adresse>
    {
    }
    public interface IRepositoryInterets : IRepository<Interet>
    {
    }
}
