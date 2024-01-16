using Microsoft.EntityFrameworkCore;
using PowerTree.Maui.Interfaces;
using PowerTree.Maui.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerTree.Maui.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        //private readonly PODContext _dbContext;
        private readonly PTContext _dbContext;
        public IHierarchyRepository Hierarchies { get; private set; }
        public INodeRepository Nodes { get; private set; }
        public INodeItemRepository NodeItems { get; private set; }
        //public ILinkRepository Links { get; private set; }
        public UnitOfWork(PTContext dbContext)
        {
            _dbContext = dbContext;
            Hierarchies = new HierarchyRepository(_dbContext);
            Nodes = new NodeRepository(_dbContext);
            NodeItems = new NodeItemRepository(_dbContext);
            //Links = new LinkRepository(_dbContext);
        }

        //public UnitOfWork(PODContext context,
        //            IContactRepository contactRepository,
        //            IHierarchyRepository hierarchyRepository,
        //            INodeRepository nodeRepository,
        //            INodeItemRepository nodeItemRepository,
        //            ILinkRepository linkRepository,
        //            INoteRepository noteRepository)
        //{
        //    _dbContext = context;

        //    Contacts = contactRepository;
        //    Hierarchies = hierarchyRepository;
        //    Nodes = nodeRepository;
        //    NodeItems = nodeItemRepository;
        //    Links = linkRepository;
        //    Notes = noteRepository;

        //    //Projects = new ProjectRepository(_context);
        //}
        public async Task<int> Complete()
        {
            return _dbContext.SaveChanges();
            //return await _dbContext.SaveChangesAsync();
        }
        public void ClearTracking()
        {
            _dbContext.ChangeTracker.Clear();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
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
