using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui;
using Microsoft.Maui.Controls.Shapes;
using PowerTree.Maui.Interfaces;
using PowerTree.Maui.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace PowerTree.Maui.ViewModel
{
    public partial class TreeViewPageViewModel : ObservableObject, ITreeViewPageViewModel
    {

        //DataService service;
        //IDialogService _dialogService;
        ITreeViewService _treeViewService;
        //INavigationService _navigationService;

        [ObservableProperty]
        private string name = "";


        public TreeViewPageViewModel( ITreeViewService treeViewService)/*INavigationService navigationService,, IDialogService dialogService*/
        {
            //this._dialogService = dialogService;
            this._treeViewService = treeViewService;
           // this._navigationService = navigationService;

        }
        [RelayCommand]
        public void ItemSelected(int nodeItemId)
        {
            // Consider this an abstract method instead ?

            //var id = _treeViewService.GetEntityIdByNodeItemId(nodeItemId);

            // TODO:  How to return EntityId so consumer ViewModel has it?

            // TODO: Now use this id to fetch the entity your hierarchy is tracking; in this case the URL
            // 

            //await Launcher.OpenAsync(url);
        }

        public void ReOrderItems(string? sourceItemId, string targetItemId)
        {
            // TODO: No need to wire up _TaskService from here... this serves as proof of concept.
            var x = "Debug";





        }
        public void ReOrderNodes(string? sourceNodeId, string? targetNodeId)
        {

        } 

        [RelayCommand]
        public async void DeleteFolder(int nodeId)
        {
            var removed = _treeViewService.RemoveNode(nodeId);

            if (!removed)
            {
                // Pop message to tell user they cannot delete this folder
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

                //var h = _unitOfWork.Hierarchies
                //            .Find(x => x.HierarchyName == HierarchySubsystemEnum.Favorites.ToString())
                //            .First();


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
        [RelayCommand]
        public async void AddItem(int nodeId)
        {
            var itemName = await Application.Current!.MainPage!.DisplayPromptAsync("New Item", "Enter the new item name:", "OK", "Cancel", "");
            if (itemName != null)
            {
                var itemURL = await Application.Current!.MainPage!.DisplayPromptAsync(itemName, "Enter the URL:", "OK", "Cancel", "https://");
                if (itemURL != null)
                {
                    //Link l = new Link()
                    //{
                    //    LinkName = itemName,
                    //    LinkURL = itemURL
                    //};

                    PTNodeItem ni = new PTNodeItem()
                    {
                        NodeId = nodeId,
                        Order = 1
                    };
                    //ni.Link = l;

                    _treeViewService.AddNodeItem(ni);

                    ReloadPage();
                }
            }
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

                //ReloadPage();
            }

        }

        [RelayCommand]
        public void DeleteItem(int nodeItemId)
        {
            _treeViewService.RemoveNodeItem(nodeItemId);

            ReloadPage();

        }


        //private RelayCommand? renameItemCommand;
        //public IRelayCommand RenameItemCommand => renameItemCommand ??= new RelayCommand(Rename_Item);

        [RelayCommand]
        public async void RenameItem(int nodeItemId)
        {
            //var l = _treeViewService.GetLinkByNodeItemId(nodeItemId);
            var ni = _treeViewService.GetNodeItem(nodeItemId);

            var newName = await Application.Current!.MainPage!.DisplayPromptAsync("Rename Item", "Enter the new item name:", "OK", "Cancel", ni.NodeItemName);

            _treeViewService.RenameEntity(nodeItemId, newName);
        }

        public void ReloadPage()
        {
            // TODO: Consider using NavigationStack to accomplish my routing instead of AppShell
            //var page = Application.Current.MainPage; //.Navigation.NavigationStack .LastOrDefault();
            // Remove old page
            //Application.Current.MainPage.Navigation.RemovePage(page);


            // BUG:  Even though this is a Transient page, there appears to be no way for a page to reload itself.
            //       The below command does not reload the page.
            //await Shell.Current.GoToAsync(nameof(TreeViewPage), false);
            //await Shell.Current.GoToAsync($"//TreeViewPage", false);



            // Currently the AppShel.xaml.cs has a workaround that reconstructs a Transient page when you navigate away from it, and then back to it.
            // However if you want to just re-initialize your current page, without navigating away from it, there appears to be no way to do this.
            // In my case, the page is the first page of the AppShell and so using the following command to rebuild the entire AppShell works.
            
            
            // TODO: Does not work from within class library
            //Application.Current!.MainPage = new AppShell();




        }

    }
}
