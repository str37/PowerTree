using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PowerTree.Maui.Interfaces;
using PowerTree.Maui.Model;
using PowerTree.Sample.Interfaces;
using PowerTree.Sample.Models;


namespace PowerTree.Sample.ViewModel
{
    public partial class FavoriteLinksViewModel: ObservableObject, ITreeViewPageViewModel
    {
        private PowerTree.Maui.Interfaces.ITreeViewService _treeViewService;
        ILinkService _linkService;

        public FavoriteLinksViewModel(PowerTree.Maui.Interfaces.ITreeViewService treeViewService, ILinkService linkService)
        {
            this._linkService = linkService;
            this._treeViewService = treeViewService;


        }
        [RelayCommand]
        public async void ItemSelected(int nodeItemId)
        {
            var entityId = _treeViewService.GetEntityIdByNodeItemId(nodeItemId);

            var l = _linkService.GetLink(entityId);
            await Launcher.OpenAsync(l.LinkURL);

        }
        [RelayCommand]
        public async void AddItem(int nodeId)
        {
            var itemName = await Application.Current!.MainPage!.DisplayPromptAsync("New Item", "Enter the new item name:", "OK", "Cancel", "");
            if (itemName != null)
            {
                var itemURL = await Application.Current!.MainPage!.DisplayPromptAsync(itemName, "Enter the URL:", "OK", "Cancel", "https://");
                if (itemURL != null)
                {
                    Link l = new Link()
                    {
                        LinkName = itemName,
                        LinkURL = itemURL
                    };
                    await _linkService.CreateLink(l);

                    PTNodeItem ni = new PTNodeItem()
                    {
                        NodeId = nodeId,
                        NodeItemName = l.LinkName,
                        Order = 1,
                        EntityId = l.LinkId,
                        NodeImage = l.LinkIcons?.FirstOrDefault()?.IconImage
                    };
                    


                    _treeViewService.AddNodeItem(ni);

                    ReloadPage();
                }
            }



        }

        [RelayCommand]
        public async void RenameItem(int nodeItemId)
        {
            var ni = _treeViewService.GetNodeItem(nodeItemId);

            var newName = await Application.Current!.MainPage!.DisplayPromptAsync("Rename Item", "Enter the new item name:", "OK", "Cancel", ni.NodeItemName);

            _treeViewService.RenameEntity(nodeItemId, newName);
        }

        [RelayCommand]
        public async void DeleteFolder(int nodeId)
        {
            var removed = _treeViewService.RemoveNode(nodeId);

            if (!removed)
            {
                Application.Current?.MainPage?.Dispatcher.Dispatch(async () =>
                    await Application.Current.MainPage.DisplayAlert("Warning", "Folder must be empty and not the only RootNode", "OK"));
            }
            else
            {
                ReloadPage();
            }
        }
        [RelayCommand]
        public async void AddFolder(int parentNodId)
        {

            var folderName = await Application.Current!.MainPage!.DisplayPromptAsync("New Folder", "Enter the new folder name:", "OK", "Cancel", ""); ;

            if (folderName != null)
            {
                PTNode pn = _treeViewService.GetNode(parentNodId);

                var n = new PTNode()
                {
                    NodeName = folderName,
                    NodeOrder = 1,
                    ParentNodeId = parentNodId,
                    HierarchyId = pn.HierarchyId
                };

                _treeViewService.AddNode(n);


                ReloadPage();

            }
        }

        public void ReOrderNodes(string? sourceNodeId, string targetNodeId)
        {
            int s = Convert.ToInt32(sourceNodeId);
            int t = Convert.ToInt32(targetNodeId);

            _treeViewService.ReOrderNodes(s, t);

            ReloadPage();

        }

        public void ReOrderItems(string? sourceItemId, string targetItemId)
        {
            int s = Convert.ToInt32(sourceItemId);
            int t = Convert.ToInt32(targetItemId);

            _treeViewService.ReOrderItems(s, t);

            ReloadPage();

        }


        [RelayCommand]
        public async void AddRootFolder(int nodeId)
        {
            var folderName = await Application.Current!.MainPage!.DisplayPromptAsync("New Folder", "Enter the new folder name:", "OK", "Cancel", null);
            if (folderName != null)
            {
                PTNode pn = _treeViewService.GetNode(nodeId);

                var n = new PTNode()
                {
                    NodeName = folderName,
                    NodeOrder = 1,
                    ParentNodeId = null,
                    HierarchyId = pn.HierarchyId
                };

                _treeViewService.AddNode(n);

                ReloadPage();
            }

        }
        [RelayCommand]
        public void DeleteItem(int nodeItemId)
        {
            _treeViewService.RemoveNodeItem(nodeItemId);

            // TODO: Decide on if you want to also delete the actual Link, or keep it
            //  Sometimes a link may belong to multiple nodes in which case you want to keep the link
            //  This means the Consumer of the PowerTree will still have to display and support managing Links that may not be in the PowerTree

            ReloadPage();

        }
        public virtual void ReloadPage()
        {
            // TODO:  This is just a hack.  Ideally we would just reload this specific page but currently I am not sure how to accomplish, possibly a bug in MAUI.
            Application.Current!.MainPage = new AppShell();




        }

    }
}
