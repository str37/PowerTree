
using PowerTree.Sample.Interfaces;
using PowerTree.Sample.Models;

namespace PowerTree.Sample.Repositories
{
    public class LinkRepository : GenericRepository<Link>, ILinkRepository
    {

        #region Xtors
        private bool _disposed = false;

        public LinkRepository(PODContext context) : base(context)
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
