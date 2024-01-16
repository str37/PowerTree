
namespace PowerTree.Sample.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ILinkRepository Links { get; }
        ILinkIconRepository LinkIcons { get; }

        Task<int> Complete();

        void ClearTracking();
    }
}
