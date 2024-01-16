using Microsoft.Maui.Controls.Shapes;
using PowerTree.Maui.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerTree.Maui.Interfaces
{
    public interface ITreeViewService
    {
        int RegisterHierarchy(string subSystem, string hierarchyName);
        bool UpdateHierarchyName(int hierarchyId, string newHierarchyName);
        bool RemoveHierarchyRegistration(int hierarchyId);

        List<PTHierarchy> GetHierarchiesBySubsystem(string subsystemName);

        PTHierarchy GetHierarchyById(int hierarchyId);

        int HierarchyId { set; }

        //PTTreeHierarchy GetTreeHierarchy(int hierarchyId);

        List<PTItemNode> GetItemNodes();
        IEnumerable<PTGroupFolder> GetGroupFolders();
        //TreeHierarchy GetTreeHierarchy();

        //string GetLinkUrlByNodeItemId(int nodeItemId);
        int GetEntityIdByNodeItemId(int nodeItemId);

        PTNodeItem GetNodeItem(int nodeItemId);
        bool RemoveNode(int nodeId);
        //Link GetLinkByNodeItemId(int nodeItemId);
        void RenameEntity(int nodeItemId, string newEntityName);

        int RemoveNodeItem(int nodeItemId);
        int AddNode(PTNode node);

        int AddNodeItem(PTNodeItem nodeItem);
        PTNode GetNode(int parentNodId);

        void ReOrderItems(int sourceItemId, int targetItemId);
        void ReOrderNodes(int sourceNodeId, int targetNodeId);
    }
}
