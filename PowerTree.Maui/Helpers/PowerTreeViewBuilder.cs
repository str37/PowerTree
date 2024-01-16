using PowerTree.Maui.Interfaces;
using PowerTree.Maui.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerTree.Maui.Helpers
{
    public class PowerTreeViewBuilder
    {
        /// <summary>
        ///  This is the Recursion that is needed to build the XamlItemGroups along with the respective XamlItems
        /// </summary>
        /// <param name="group"></param>
        /// <param name="gf"></param>
        /// <returns></returns>
        private XamlItemGroup FindParentGroupFolder(XamlItemGroup group, PTGroupFolder gf)
        {
            if (group.GroupId == gf.ParentGroupFolderId)
                return group;

            if (group.Children != null)
            {
                foreach (var currentGroup in group.Children)
                {
                    var search = FindParentGroupFolder(currentGroup, gf);

                    if (search != null)
                        return search;
                }
            }

            return null;
        }

        // This method is where we populate the data structure with names and Id's
        /// <summary>
        /// The injected service will retrieve that data from the database related 
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        public XamlItemGroup GroupData(ITreeViewService service)
        {
            //var treeHierarchy = service.GetTreeHierarchy();
            var groupFolders = service.GetGroupFolders().OrderBy(x => x.ParentGroupFolderId);
            var itemNodes = service.GetItemNodes();

            var companyGroup = new XamlItemGroup();
            companyGroup.Name = "Hierarchy"; 

            foreach (var dept in groupFolders)
            {
                var itemGroup = new XamlItemGroup();
                itemGroup.Name = dept.GroupFolderName;
                itemGroup.GroupId = dept.GroupFolderId;

                // Employees first
                var employeesDepartment = itemNodes.Where(x => x.GroupFolderId == dept.GroupFolderId);

                foreach (var emp in employeesDepartment)
                {
                    var item = new XamlItem();
                    item.ItemId = emp.ItemNodeId;
                    item.Key = emp.ItemNodeName;
                    item.Icon = emp.IconImage;

                    itemGroup.XamlItems.Add(item);
                }

                // Departments now
                if (dept.ParentGroupFolderId == -1)
                {
                    companyGroup.Children.Add(itemGroup);
                }
                else
                {
                    XamlItemGroup parentGroup = null;

                    foreach (var group in companyGroup.Children)
                    {
                        parentGroup = FindParentGroupFolder(group, dept);

                        if (parentGroup != null)
                        {
                            parentGroup.Children.Add(itemGroup);
                            break;
                        }
                    }
                }
            }

            return companyGroup;
        }
    }
}
