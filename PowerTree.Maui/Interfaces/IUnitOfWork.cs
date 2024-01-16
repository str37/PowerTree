using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerTree.Maui.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IHierarchyRepository Hierarchies { get; }
        INodeRepository Nodes { get; }
        INodeItemRepository NodeItems { get; }
        //ILinkRepository Links { get; }
        //IProjectRepository Projects { get; }
        //int Complete();
        Task<int> Complete();
        void ClearTracking();
    }
}
