using Microsoft.EntityFrameworkCore;
using PowerTree.Maui.Interfaces;
using PowerTree.Maui.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerTree.Maui.Repositories
{
    public class HierarchyRepository : GenericRepository<PTHierarchy>, IHierarchyRepository
    {
        #region Beyond the Generic Repository Methods
        public PTHierarchy GetHierarchyBySubsystem(string subSystem)
        {
            // TODO: Consider using Set<T> 
            //return _context.Set<T>().Hierar
            //return _context.Hierarchies
            //    .Where(x => x.Subsystem == subSystem)
            //    .FirstOrDefault();
            return null;
            //return _context.Contacts.OrderByDescending(d => d.IsFavorite).Take(count).ToList();
        }




        #endregion


        #region Xtors
        private bool _disposed = false;

        public HierarchyRepository(PTContext context) : base(context)
        {
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        #endregion

    }
}
