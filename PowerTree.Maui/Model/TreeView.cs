using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerTree.Maui.Model
{
    public class PTTreeHierarchy
    {
        public int TreeHierarchyId { get; set; }
        public string TreeHierarchyName { get; set; }
    }
    public class PTGroupFolder
    {
        public int GroupFolderId { get; set; }
        public string GroupFolderName { get; set; }
        public int ParentGroupFolderId { get; set; }
        public int TreeHierarchyId { get; set; }
    }

    public class PTItemNode
    {
        public int ItemNodeId { get; set; }
        public string ItemNodeName { get; set; }
        public int GroupFolderId { get; set; }

        public byte[]? IconImage { get; set; }
    }
}
