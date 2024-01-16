using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Platform;
using PowerTree.Maui.Interfaces;
using PowerTree.Maui.Model;
using PowerTree.Maui.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace PowerTree.Maui.Services
{
    public class PTTreeViewService : ITreeViewService
    {
        private readonly IUnitOfWork _unitOfWork;
        private int hierarchyId;

        public int HierarchyId 
        { 
            set { hierarchyId = value; }
        }

        public PTTreeViewService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }
        public void ReOrderNodes(int sourceNodeId, int targetNodeId)
        {
            // Source and Target may belong to different parent nodes as Nodes may moved at different levels
            // Get the two items, source and target
            var l = _unitOfWork.Nodes.Find(x => x.NodeId == sourceNodeId || x.NodeId == targetNodeId).ToList();
            int? targetParentNodeId = 0;
            PTNode sourceNode = l.Find(x => x.NodeId == sourceNodeId);
            if (l[0].ParentNodeId == l[1].ParentNodeId)
            {
                targetParentNodeId = l[1].ParentNodeId; //  Nodes have same Parent node
            }
            else
            {
                targetParentNodeId = l.Find(x => x.NodeId == targetNodeId).ParentNodeId;//  Items have different Parent node

                // Set the source Node ParentNodeId to the new targetParentNodeId
                sourceNode.ParentNodeId = targetParentNodeId;
                _unitOfWork.Nodes.Update(sourceNode);
                _unitOfWork.Complete();

            }
            _unitOfWork.ClearTracking();


            // The assumption is that all nodes already have an order Number correctly set during "Add" operations
            //  i.e.   1 through x; potentially with some gaps for deleted or moved items
            var nodeList = _unitOfWork.Nodes.Find(x => x.ParentNodeId == targetParentNodeId).OrderBy(y => y.NodeOrder).ToList();
            List<PTNode> newList = new List<PTNode>();
            int counter = 0;
            int insertionPoint = 0;
            foreach (var item in nodeList)
            {
                if (item.NodeId == targetNodeId)
                {
                    insertionPoint = counter;

                    //newList.Add(sourceNode);
                    newList.Add(item);
                }
                else if (item.NodeId == sourceNodeId)
                {
                    sourceNode = item;

                   // newList.Add(sourceNode);
                    //newList.Add(item);
                }
                else
                {
                    newList.Add(item);
                }
                counter++;
            }

            newList.Insert(insertionPoint, sourceNode);



            // Now set the 'Order' column of each item in list
            counter = 1;
            foreach (var i in newList)
            {
                _unitOfWork.Nodes.Update(i);

                i.NodeOrder = counter;
                counter++;
            }


            _unitOfWork.Complete();
            _unitOfWork.ClearTracking();

        }



        public void ReOrderItems(int sourceItemId, int targetItemId)
        {
            // Source and Target may belong to different nodes as items may move among nodes
            // Get the two items, source and target
            var l = _unitOfWork.NodeItems.Find(x => x.NodeItemId == sourceItemId || x.NodeItemId == targetItemId).ToList();
            int targetNodeId = 0;
            PTNodeItem sourceNodeItem = l.Find(x => x.NodeItemId == sourceItemId);
            bool IsNodeMove = false;
            if (l[0].NodeId == l[1].NodeId) 
            {
                targetNodeId = l[1].NodeId; //  Items are in same node
            }
            else 
            {
                IsNodeMove = true;
                targetNodeId = l.Find(x => x.NodeItemId == targetItemId).NodeId;//  Items are in different node

                // Set the source item to the new target Node
                sourceNodeItem.NodeId = targetNodeId;
                _unitOfWork.NodeItems.Update(sourceNodeItem);
                _unitOfWork.Complete();

            }
            _unitOfWork.ClearTracking(); 

            // The assumption is that all items already have an order Number correctly set during "Add" operations
            //  i.e.   1 through x; potentially with some gaps for deleted or moved items
            var itemList = _unitOfWork.NodeItems.Find(x => x.NodeId == targetNodeId).OrderBy(y => y.Order).ToList(); 
            List<PTNodeItem> newList = new List<PTNodeItem>();
            int counter = 0;
            int insertionPoint = 0;
            foreach (var item in itemList)
            {
                if (item.NodeItemId == targetItemId)
                {
                    insertionPoint = counter;
                    //newList.Add(sourceNodeItem);
                    newList.Add(item);
                }
                else if (item.NodeItemId == sourceItemId)
                {
                    sourceNodeItem = item;
                    //newList.Add(sourceNodeItem);
                    //newList.Add(item);
                }
                else
                {
                    newList.Add(item);
                }
                counter++;
            }

            newList.Insert(insertionPoint, sourceNodeItem);


            // Now set the 'Order' column of each item in list
            counter = 1;
            foreach (var i in newList)
            {
                _unitOfWork.NodeItems.Update(i);
                i.Order = counter;
                counter++;
            }


            _unitOfWork.Complete();
            _unitOfWork.ClearTracking();

        }
       
        public int RegisterHierarchy(string subSystem, string hierarchyName)
        { 
            var result = _unitOfWork.Hierarchies.Find(x => x.Subsystem == subSystem && x.HierarchyName == hierarchyName).FirstOrDefault();
            if (result != null) { return 0; }

            var h = new PTHierarchy() { HierarchyName = hierarchyName, Subsystem = subSystem };
            // Also add a single root node to allow for context menu for adding other rootnodes
            var rootNode = new PTNode() { Hierarchy = h, NodeName = "RootNode", NodeOrder = 1  }; 
            _unitOfWork.Nodes.Add(rootNode);
            _unitOfWork.Complete();
            _unitOfWork.ClearTracking();
            return h.HierarchyId;
        }
        public bool UpdateHierarchyName(int hierarchyId, string newHierarchyName)
        {
            var h = _unitOfWork.Hierarchies.Find(x => x.HierarchyId == hierarchyId).First();
            if (h == null) { return false; }

            h.HierarchyName = newHierarchyName;
            _unitOfWork.Hierarchies.Update(h);
            _unitOfWork.Complete();
            _unitOfWork.ClearTracking();
            return true;
        }
        
        public bool RemoveHierarchyRegistration(int hierarchyId)
        {
            try
            {
                _unitOfWork.Hierarchies.Remove(new PTHierarchy() { HierarchyId = hierarchyId });
                _unitOfWork.Complete();
                

            }
            catch (Exception)
            {

                return false;
            }

            return false;
        }


        public List<PTHierarchy> GetHierarchiesBySubsystem(string subsystemName)
        {
            return _unitOfWork.Hierarchies.Find(x => x.Subsystem == subsystemName).ToList();

        }

        public PTHierarchy GetHierarchyById(int hierarchyId)
        {
            return _unitOfWork.Hierarchies.Find(x => x.HierarchyId != hierarchyId).FirstOrDefault();
        }


        //public PTTreeHierarchy GetTreeHierarchy()
        //{
        //    var h = _unitOfWork.Hierarchies.Find(x => x.HierarchyName == SubSystem.ToString()).FirstOrDefault();
        //    hierarchyId = h.HierarchyId;

        //    var th = new PTTreeHierarchy()
        //    {
        //        TreeHierarchyId = h.HierarchyId,
        //        TreeHierarchyName = h.HierarchyName
        //    };
        //    return th;
        //}

        //public PTTreeHierarchy GetHierarchyById(int hierarchyId)
        //{
        //    var h = _unitOfWork.Hierarchies.Find(x => x.HierarchyId == hierarchyId).FirstOrDefault();

        //    var th = new PTTreeHierarchy()
        //    {
        //        TreeHierarchyId = h.HierarchyId,
        //        TreeHierarchyName = h.HierarchyName
        //    };
        //    return th;
        //}
        public PTNode GetNode(int parentNodId)
        {
            return _unitOfWork.Nodes.Find(x => x.NodeId == parentNodId).FirstOrDefault();
        }
        public int RemoveNodeItem(int nodeItemId)
        {
            var ni = _unitOfWork.NodeItems.Find(x => x.NodeItemId == nodeItemId).First();
            var id = ni.NodeItemId;


            //_unitOfWork.Links.Remove(new Link { LinkId = (int)ni.LinkId });
            _unitOfWork.NodeItems.Remove(ni);
            _unitOfWork.Complete();

            return id;
        }
        public PTNodeItem GetNodeItem(int nodeItemId)
        {
            return _unitOfWork.NodeItems.Find(x => x.NodeItemId == nodeItemId).First();
        }
        public void RenameEntity(int nodeItemId, string newEntityName)
        {

            var ni = _unitOfWork.NodeItems.Find(x => x.NodeItemId == nodeItemId).First();
            ni.NodeItemName = newEntityName;
            _unitOfWork.Complete();
            _unitOfWork.ClearTracking();


        }
        public int GetEntityIdByNodeItemId(int nodeItemId)
        {
            var ni = _unitOfWork.NodeItems.GetById(nodeItemId);
            _unitOfWork.ClearTracking();
            return ni.EntityId;
        }
        public int AddNodeItem(PTNodeItem nodeItem)
        {
            // TODO:  Perhaps use specification pattern to optimize this and only get last one from DB instead of all items
            // This block will  insert a new NodeItem with an 'Order' at the end of all other NodeItems
            var l = _unitOfWork.NodeItems.Find(x => x.NodeId == nodeItem.NodeId).OrderBy(x => x.Order).TakeLast(1).FirstOrDefault();
            if (l != null)
            {
                nodeItem.Order = l.Order + 1;
            }

            _unitOfWork.NodeItems.Add(nodeItem);
            _unitOfWork.Complete();
            _unitOfWork.ClearTracking();
            return nodeItem.NodeItemId;
        }
        public int AddNode(PTNode node)
        {
            // This block will insert a new Node with a 'NodeOrder' at the end of all other Nodes
            var l = _unitOfWork.Nodes.Find(x => x.ParentNodeId == node.ParentNodeId).OrderBy(x => x.NodeOrder).TakeLast(1).FirstOrDefault();
            if (l != null)
            {
                node.NodeOrder = l.NodeOrder + 1;
            }


            _unitOfWork.Nodes.Add(node);
            _unitOfWork.Complete();
            _unitOfWork.ClearTracking();
            return node.NodeId;
        }
        public bool RemoveNode(int nodeId)
        {
            var q = _unitOfWork.Nodes.Find(x => x.ParentNodeId == nodeId);
            if (q == null || q.Count() == 0)
            {
                var q2 = _unitOfWork.NodeItems.Find(x => x.NodeId == nodeId);

                if (q2 == null || q2.Count() == 0)
                {
                    var q3 = _unitOfWork.Nodes.Find(x => x.NodeId == nodeId && x.ParentNodeId == null);

                    if (q3 == null || q3.Count() == 0) // This is not a Root Node so go ahead and delete
                    {
                        _unitOfWork.Nodes.Remove(new PTNode { NodeId = nodeId });
                        _unitOfWork.Complete();
                        return true;
                    }
                    else // This is a root node, so make sure it is not the last one
                    {
                        var q4 = _unitOfWork.Nodes.Find(x => x.ParentNodeId == null);

                        if (q4.Count() > 1) // Not the last root node
                        {
                            _unitOfWork.Nodes.Remove(new PTNode { NodeId = nodeId });
                            _unitOfWork.Complete();

                            return true;
                        }
                        else // Last Root Node(Dont delete)
                        {
                            return false;
                        }
                    }
                }

            }
            return false;

        }
        public IEnumerable<PTGroupFolder> GetGroupFolders()
        {

            var nL = _unitOfWork.Nodes.Find(x => x.HierarchyId == hierarchyId).OrderBy(x => x.ParentNodeId).ThenBy(x => x.NodeOrder);

            var l = new List<PTGroupFolder>();
            foreach (var item in nL)
            {
                var gf = new PTGroupFolder()
                {
                    TreeHierarchyId = item.HierarchyId,
                    GroupFolderId = item.NodeId,
                    ParentGroupFolderId = item.ParentNodeId ??= -1, // The data population of tree expects -1 for no parent
                    GroupFolderName = item.NodeName
                };

                l.Add(gf);
            }

            return l;

            //return new List<GroupFolder>()
            //{
            //    new GroupFolder() { TreeHierarchyId = 1, GroupFolderId = 1, GroupFolderName = "IT", ParentGroupFolderId = -1 },
            //    new GroupFolder() { TreeHierarchyId = 1,  GroupFolderId = 2, GroupFolderName = "Accounting", ParentGroupFolderId = -1 },
            //    new GroupFolder() { TreeHierarchyId = 1,  GroupFolderId = 3, GroupFolderName = "Production", ParentGroupFolderId = -1 },
            //    new GroupFolder() { TreeHierarchyId = 1,  GroupFolderId = 4, GroupFolderName = "Software", ParentGroupFolderId = 1 },
            //    new GroupFolder() { TreeHierarchyId = 1,  GroupFolderId = 5, GroupFolderName = "Support", ParentGroupFolderId = 1 },
            //    new GroupFolder() { TreeHierarchyId = 1,  GroupFolderId = 6, GroupFolderName = "Testing", ParentGroupFolderId = 4 },
            //    new GroupFolder() { TreeHierarchyId = 1,  GroupFolderId = 7, GroupFolderName = "Accounts receivable", ParentGroupFolderId = 2 },
            //    new GroupFolder() { TreeHierarchyId = 1,  GroupFolderId = 8, GroupFolderName = "Accounts payable", ParentGroupFolderId = 2 },
            //    new GroupFolder() { TreeHierarchyId = 1,  GroupFolderId = 9, GroupFolderName = "Customers and services", ParentGroupFolderId = 8 }
            //};
        }

        public List<PTItemNode> GetItemNodes()
        {
            //var specification = new AllNodeItemsWithLinkSpecification(hierarchyId);
            var list = _unitOfWork.NodeItems.Find(x => x.Node.HierarchyId == hierarchyId).OrderBy(x => x.NodeId).ThenBy(x => x.Order);

            var l = new List<PTItemNode>();
            foreach (var f in list)
            {
                var gf = new PTItemNode()
                {
                    ItemNodeId = (int)f.NodeItemId,
                    ItemNodeName = f.NodeItemName, // f.Link.LinkName,
                    GroupFolderId = (int)f.NodeId,
                    IconImage = f.NodeImage // This is the icon image
                };

                l.Add(gf);
            }

            //return l.OrderBy(x => x.GroupFolderId).ToList(); // Order by GroupFolderId is required to proper rendering of tree
            return l; // Order by GroupFolderId is required to proper rendering of tree



            //return new List<ItemNode>()
            //{
            //    new ItemNode() { ItemNodeId = 1, ItemNodeName = "Luis", GroupFolderId = 1 },
            //    new ItemNode() { ItemNodeId = 2, ItemNodeName = "Pepe", GroupFolderId = 1 },
            //    new ItemNode() { ItemNodeId = 3, ItemNodeName = "Juan", GroupFolderId = 2 },
            //    new ItemNode() { ItemNodeId = 4, ItemNodeName = "Inés", GroupFolderId = 3 },
            //    new ItemNode() { ItemNodeId = 5, ItemNodeName = "Sara", GroupFolderId = 3 },
            //    new ItemNode() { ItemNodeId = 6, ItemNodeName = "Sofy", GroupFolderId = 4 },
            //    new ItemNode() { ItemNodeId = 7, ItemNodeName = "Hugo", GroupFolderId = 5 },
            //    new ItemNode() { ItemNodeId = 8, ItemNodeName = "Gema", GroupFolderId = 5 },
            //    new ItemNode() { ItemNodeId = 9, ItemNodeName = "Olga", GroupFolderId = 6 },
            //    new ItemNode() { ItemNodeId = 1, ItemNodeName = "Otto", GroupFolderId = 6 },
            //    new ItemNode() { ItemNodeId = 2, ItemNodeName = "Axel", GroupFolderId = 6 },
            //    new ItemNode() { ItemNodeId = 3, ItemNodeName = "Eloy", GroupFolderId = 7 },
            //    new ItemNode() { ItemNodeId = 4, ItemNodeName = "Flor", GroupFolderId = 8 },
            //    new ItemNode() { ItemNodeId = 5, ItemNodeName = "Aída", GroupFolderId = 9 },
            //    new ItemNode() { ItemNodeId = 6, ItemNodeName = "Ruth", GroupFolderId = 9 }
            //};
        }
    }
}
