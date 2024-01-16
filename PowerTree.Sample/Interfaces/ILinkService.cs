
using PowerTree.Sample.Models;
using System.Collections.Generic;


namespace PowerTree.Sample.Interfaces
{
    public interface ILinkService
    {
        #region Non Async Methods
        Link GetLink(int linkId);

        void DeleteLink(int linkId);

        Task<Link> CreateLink(Link link);


        void SaveLink(Link link);



        #endregion

        #region Async Methods



        #endregion


    }
}