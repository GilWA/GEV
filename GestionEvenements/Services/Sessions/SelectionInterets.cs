using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

using Modele;

namespace GestionEvenements.Services.Sessions
{
    public class SelectionInterets
    {
        HttpContext _context;
        List<Interet> _interets;
        private const string cleSession = "Interets";
        
        public SelectionInterets(HttpContext context)
        {
            _context = context;
            _interets = _context.Session.GetObjectFromJson<List<Interet>>(cleSession) ?? new List<Interet>();
        }

        public void ActualiserInterets(List<Interet> interets)
        {
            _interets = interets;
            MajSession();
        }

        void MajSession()
        {
            _context.Session.SetObjectAsJson(cleSession, _interets);
        }
        public List<Interet> Liste()
        {
            return _interets;
        }
        public bool Existe(int id)
        {
            return  _interets.SingleOrDefault(it => it.EvenementId == id) != null;
        }
        public Interet Lire(int id)
        {
            return _interets.SingleOrDefault(it => it.EvenementId == id);
        }
        public void Ajouter(Interet interet)
        {
            if (Existe(interet.EvenementId)) return;

            _interets.Add(interet);
            MajSession();
        }
        public void Modifier(Interet interet)
        {
            var old = Lire(interet.EvenementId);
            if (old == null) return;

            _interets.Remove(old);
            _interets.Add(interet);
            MajSession();
        }
        public void Supprimer(int id)
        {
            if (!Existe(id)) return;

            var interet = Lire(id);
            _interets.Remove(interet);
            MajSession();
        }

        public void DetruireSession()
        {
            _context.Session.Clear();
            _context.Session.Remove(_context.Session.Id);
        }
    }
}
