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
    public class NodeRepository : GenericRepository<PTNode>, INodeRepository
    {

        #region Xtors
        private bool _disposed = false;

        public NodeRepository(PTContext context) : base(context)
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
