using Microsoft.Maui.Controls.Shapes;
using PowerTree.Maui.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerTree.Maui.Interfaces
{
    public interface ITreeViewPageViewModel
    {

        void ReOrderItems(string? sourceItemId, string targetItemId);
        void ReOrderNodes(string? sourceNodeId, string targetNodeId);
        void ItemSelected(int nodeItemId);


        void DeleteFolder(int nodeId);

        void AddFolder(int parentNodId);


        void AddItem(int nodeId);

        void AddRootFolder(int nodeId);

        void DeleteItem(int nodeItemId);

        void RenameItem(int nodeItemId);






    }
}
