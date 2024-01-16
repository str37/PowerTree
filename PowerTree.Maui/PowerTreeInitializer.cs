using PowerTree.Maui.Controls;
using PowerTree.Maui.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerTree.Maui
{
    public static class PowerTreeInitializer

    {
        public static PowerTree.Maui.Controls.PowerTree CreatePowerTreeControl(ITreeViewPageViewModel yourViewModel)
        {
            var tvs = new PowerTree.Maui.Services.PTTreeViewService(new PowerTree.Maui.UnitOfWork.UnitOfWork(new PowerTree.Maui.PTContext()));
            //var vm = new PowerTree.Maui.ViewModel.TreeViewPageViewModel(tvs);
            var tbuilder = new PowerTree.Maui.Helpers.PowerTreeViewBuilder();
            var zt = new PowerTree.Maui.Controls.PowerTree(yourViewModel, tvs, tbuilder);

            return zt;
        }
        /// <summary>
        /// If your subSystem has multiple Hierarchies which are tracked, you may want to send the specific hierarchyId so service knows what data to load
        /// from among multiple potential hierarchies
        /// </summary>
        /// <param name="yourViewModel"></param>
        /// <param name="hierarchyId"></param>
        /// <returns></returns>
        public static PowerTree.Maui.Controls.PowerTree CreatePowerTreeControl(ITreeViewPageViewModel yourViewModel, int hierarchyId)
        {
            var tvs = new PowerTree.Maui.Services.PTTreeViewService(new PowerTree.Maui.UnitOfWork.UnitOfWork(new PowerTree.Maui.PTContext()));
            tvs.HierarchyId = hierarchyId;

            var tbuilder = new PowerTree.Maui.Helpers.PowerTreeViewBuilder();
            var zt = new PowerTree.Maui.Controls.PowerTree(yourViewModel, tvs, tbuilder);

            return zt;
        }
        /// <summary>
        /// If your subSystem only uses one Hierarchy send the subsystem name as a parameter so the service knows what hierarchical data to load as there is only one.
        /// The service will register a new hierarchy for the subsystem if one is not present; 
        /// </summary>
        /// <param name="yourViewModel"></param>
        /// <param name="subSystem"></param>
        /// <returns></returns>
        public static PowerTree.Maui.Controls.PowerTree CreatePowerTreeControl(ITreeViewPageViewModel yourViewModel, string subSystem)
        {
            var tvs = new PowerTree.Maui.Services.PTTreeViewService(new PowerTree.Maui.UnitOfWork.UnitOfWork(new PowerTree.Maui.PTContext()));

            // Get or create hierarchy as needed
            var h = tvs.GetHierarchiesBySubsystem(subSystem);
            int hierarchyId = 0;
            if (h == null || h.Count == 0)
            {
                hierarchyId = tvs.RegisterHierarchy(subSystem, subSystem);
            }
            else
            {
                hierarchyId = h.First().HierarchyId;
            }
            
            tvs.HierarchyId = hierarchyId;

            //var vm = new PowerTree.Maui.ViewModel.TreeViewPageViewModel(tvs);
            var tbuilder = new PowerTree.Maui.Helpers.PowerTreeViewBuilder();
            var zt = new PowerTree.Maui.Controls.PowerTree(yourViewModel, tvs, tbuilder);

            return zt;
        }

        //public static PowerTree.Maui.Controls.PowerTree CreatePowerTreeControl()
        //{
        //    var tvs = new PowerTree.Maui.Services.PTTreeViewService(new PowerTree.Maui.UnitOfWork.UnitOfWork(new PowerTree.Maui.PTContext()));
        //    var vm = new PowerTree.Maui.ViewModel.TreeViewPageViewModel(new PowerTree.Maui.Services.PTTreeViewService(new PowerTree.Maui.UnitOfWork.UnitOfWork(new PowerTree.Maui.PTContext())));
        //    var tbuilder = new PowerTree.Maui.Helpers.PowerTreeViewBuilder();
        //    var zt = new PowerTree.Maui.Controls.PowerTree(vm, tvs, tbuilder);

        //    return zt;
        //}


        /// <summary>
        /// Use this to explore possiblity of user created Viewmodel that is passed into this method.
        /// This would give the consumer of the control full ability to act on all menu clicks and drag/drop events
        /// This consumer created ViewModel might inherit from PowerTree.Maui.ViewModel.TreeViewPageViewModel or implement its interface.
        /// (Problably inheritence is best as we get to extend the implementation, and still call the base class implementation)
        ///   If we pass in this consumer ViewModel, we can set the Control Views BindingContext to this consumer ViewModel
        ///      i.e.   BindingContext = new ConsumerViewModel(...)
        ///      


    }
}
