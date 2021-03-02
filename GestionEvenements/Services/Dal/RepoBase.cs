using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace GestionEvenements.Services.Dal
{
    public class RepoBase
    {
        private readonly GevDbContext _context;
        public GevDbContext Context => _context;
        public RepoBase(GevDbContext context)
        {
            _context = context;
        }
       
        protected async Task<int> Enregistrer(Object entite, EntityState state)
        {
            if (entite == null) return 0; // On pourrait lever une Exception
            _context.Entry(entite).State = state;
            return await _context.SaveChangesAsync();
        }

        protected void NoTracking()
        {
            _context.ChangeTracker.AutoDetectChangesEnabled = false;
            _context.ChangeTracker.LazyLoadingEnabled = false;
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }
    }
}
