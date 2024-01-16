
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PowerTree.Sample.Interfaces;
using PowerTree.Sample.Repositories;


namespace PowerTree.Sample.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PODContext _dbContext;
        public ILinkRepository Links { get; private set; }
        public ILinkIconRepository LinkIcons { get; private set; }

        public UnitOfWork(PODContext dbContext)
        {
            _dbContext = dbContext;
            Links = new LinkRepository(_dbContext);

        }

        public async Task<int> Complete()
        {
            return _dbContext.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public void ClearTracking()
        {
            _dbContext.ChangeTracker.Clear();
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
        }
    }
}
