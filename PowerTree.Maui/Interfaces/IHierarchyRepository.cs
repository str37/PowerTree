using PowerTree.Maui.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerTree.Maui.Interfaces
{
    public interface IHierarchyRepository : IGenericRepository<PTHierarchy>
    {
        //IEnumerable<Contact> GetFavoriteContacts(int count);
        //void DeleteContact(int contactId);
        PTHierarchy GetHierarchyBySubsystem(string subSystem);
    }
}
