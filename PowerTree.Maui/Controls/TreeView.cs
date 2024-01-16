using CommunityToolkit.Maui.Markup;
using PowerTree.Maui.Interfaces;
using PowerTree.Maui.Model;
using PowerTree.Maui.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Binding = Microsoft.Maui.Controls.Binding;
using BindingMode = Microsoft.Maui.Controls.BindingMode;

namespace PowerTree.Maui.Controls
{
    public class TreeView : ScrollView
    {



        private readonly StackLayout _StackLayout = new StackLayout { Orientation = StackOrientation.Vertical };

        //TODO: This initialises the list, but there is nothing listening to INotifyCollectionChanged so no nodes will get rendered
        private IList<TreeViewNode> _RootNodes = new ObservableCollection<TreeViewNode>();
        private TreeViewNode _SelectedItem;

        /// <summary>
        /// The item that is selected in the tree
        /// TODO: Make this two way - and maybe eventually a bindable property
        /// </summary>
        public TreeViewNode SelectedItem
        {
            get => _SelectedItem;

            set
            {
                if (_SelectedItem == value)
                {
                    return;
                }

                if (_SelectedItem != null)
                {
                    _SelectedItem.IsSelected = false;
                }

                _SelectedItem = value;

                SelectedItemChanged?.Invoke(this, new EventArgs());
            }
        }


        public IList<TreeViewNode> RootNodes
        {
            get => _RootNodes;
            set
            {
                _RootNodes = value;

                if (value is INotifyCollectionChanged notifyCollectionChanged)
                {
                    notifyCollectionChanged.CollectionChanged += (s, e) =>
                    {
                        RenderNodes(_RootNodes, _StackLayout, e, null);
                    };
                }

                RenderNodes(_RootNodes, _StackLayout, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset), null);
            }
        }

        /// <summary>
        /// Occurs when the user selects a TreeViewItem
        /// </summary>
        public event EventHandler SelectedItemChanged;

        public TreeView()
        {
            Content = _StackLayout;

            // Uncomment for Testing of Binding within TreeView Control
            //var btn = new Button()
            //{
            //    Text = "Binding Test Button",
            //    WidthRequest = 160 

            //};

            //RelativeBindingSource rbs = new RelativeBindingSource(RelativeBindingSourceMode.FindAncestorBindingContext, ancestorType: typeof(TreeViewPageViewModel));

            //btn.Bind(Button.CommandProperty,
            //    mode: BindingMode.OneTime,
            //    source: rbs,
            //    path: "RenameItemCommand"
            //    );
            //_StackLayout.Add(btn);


        }

        private void RemoveSelectionRecursive(IEnumerable<TreeViewNode> nodes)
        {
            foreach (var treeViewItem in nodes)
            {
                if (treeViewItem != SelectedItem)
                {
                    treeViewItem.IsSelected = false;
                }

                RemoveSelectionRecursive(treeViewItem.ChildrenList);
            }
        }

        private static void AddItems(IEnumerable<TreeViewNode> childTreeViewItems, StackLayout parent, TreeViewNode parentTreeViewItem)
        {
            foreach (var childTreeNode in childTreeViewItems)
            {
                if (!parent.Children.Contains(childTreeNode))
                {
                    parent.Children.Add(childTreeNode);
                }

                childTreeNode.ParentTreeViewItem = parentTreeViewItem;
            }
        }

        /// <summary>
        /// TODO: A bit stinky but better than bubbling an event up...
        /// </summary>
        internal void ChildSelected(TreeViewNode child)
        {
            SelectedItem = child;
            child.IsSelected = true;
            child.SelectionBoxView.Color = child.SelectedBackgroundColor;
            child.SelectionBoxView.Opacity = child.SelectedBackgroundOpacity;
            RemoveSelectionRecursive(RootNodes);
        }

        internal static void RenderNodes(IEnumerable<TreeViewNode> childTreeViewItems, StackLayout parent, NotifyCollectionChangedEventArgs e, TreeViewNode parentTreeViewItem)
        {
            if (e.Action != NotifyCollectionChangedAction.Add)
            {
                //TODO: Reintate this...
                //parent.Children.Clear();
                AddItems(childTreeViewItems, parent, parentTreeViewItem);
            }
            else
            {
                AddItems(e.NewItems.Cast<TreeViewNode>(), parent, parentTreeViewItem);
            }
        }

        // Main code: 
        private TreeViewNode CreateTreeViewNode(object bindingContext, Label label, bool isItem)
        {
            // Create a ContentView around the Label so we may let the DragOver/DragLeave create an animation to show where the Item is being dropped
            var cv = new ContentView() { BackgroundColor=Colors.White, VerticalOptions= LayoutOptions.Center, Padding= new Thickness(0,3,0,0) };
            cv.Content = label;

            XamlItem item = null;
            if (isItem)
            {
                item = (XamlItem)bindingContext;
            }

            ResourceImage ri = new ResourceImage();
            ri.HeightRequest = 16;
            ri.WidthRequest = 16;
            ri.Resource = "folderopen.png";

            Image itemImage = new Image();
            if (isItem && item.Icon != null && item.Icon.Count() > 0)
            {
                itemImage.HeightRequest = 16;
                itemImage.WidthRequest = 16;
                itemImage.Source =  ImageSource.FromStream( () => new MemoryStream(item.Icon) );
            }

            StackLayout sl = new StackLayout();
            sl.Orientation = StackOrientation.Horizontal;

            if (isItem && item.Icon != null && item.Icon.Count() > 0)
            {
                sl.Children.Add(itemImage);
            }
            else if (isItem)
            {
                ri.Resource = "item.png";
                sl.Children.Add(ri);
            }
            else
            {
                sl.Children.Add(ri);
            }
            sl.Children.Add(cv);

            var node = new TreeViewNode
            {
                BindingContext = bindingContext,
                Content = sl,
                //Content = new StackLayout
                //{
                //    Children = 
                //    {
                //        new ResourceImage
                //        {

                //            Resource = isItem? "item.png" :"folderopen.png" ,
                //            HeightRequest= 16,
                //            WidthRequest = 16
                //        },
                //        ri,
                //        cv // was label
                //    },
                //    Orientation = StackOrientation.Horizontal
                //}
            };

            //set DataTemplate for expand button content
            node.ExpandButtonTemplate = new DataTemplate(() => new ExpandButtonContent { BindingContext = node });

            return node;
        }

        private void CreateXamlItem(IList<TreeViewNode> children, XamlItem xamlItem)
        {
            var label = new Label
            {
                VerticalOptions = LayoutOptions.Center,
                TextColor = Colors.Black,
                BackgroundColor = Colors.White


            };
            // ================== This is where we want to put the Flyout Context Menu for the Item ===========================================================

            RelativeBindingSource rbs = new RelativeBindingSource(RelativeBindingSourceMode.FindAncestorBindingContext, ancestorType: typeof(ITreeViewPageViewModel));

            var menuItem1 = new MenuFlyoutItem
            {
                Text = "Delete Item",
                CommandParameter = xamlItem.ItemId
            };

            menuItem1.Bind(MenuFlyoutItem.CommandProperty,
                mode: BindingMode.OneTime,
                source: rbs,
                path: "DeleteItemCommand"
            );

            var sep = new MenuFlyoutSeparator();

            var menuItem2 = new MenuFlyoutItem
            {
                Text = "Rename Item",
                CommandParameter = xamlItem.ItemId
            };
            menuItem2.Bind(MenuFlyoutItem.CommandProperty,
                mode: BindingMode.OneTime,
                source: rbs,
                path: "RenameItemCommand"
            );

            var menuFlyout = new MenuFlyout();

            menuFlyout.Add(menuItem1);
            menuFlyout.Add(sep);
            menuFlyout.Add(menuItem2);

            FlyoutBase.SetContextFlyout(label, menuFlyout);



            label.SetBinding(Label.TextProperty, "Key");
            // This is required for the Drag/Drop events to be able to process the event arguments
            // by retrieving the ItemId 
            label.StyleId = "Item_" + xamlItem.ItemId.ToString();

            // ================= Tap Gesture for Items =======================
            TapGestureRecognizer tgr = new TapGestureRecognizer
            {
                Buttons = ButtonsMask.Primary,
                CommandParameter = xamlItem.ItemId,
                NumberOfTapsRequired = 2
            };
            tgr.Bind(TapGestureRecognizer.CommandProperty,
                mode: BindingMode.OneTime,
                source: rbs,
                path: "ItemSelectedCommand"
                );

            label.GestureRecognizers.Add(tgr);

            // ================== DragStarting Gesture for Items =======================
            DragGestureRecognizer dragGr = new DragGestureRecognizer
            {
                CanDrag = true
            };
            dragGr.DragStarting += DragGr_DragStarting_Item; 

            // ================== Drop Gesture for Items ===============================
            DropGestureRecognizer dropGr = new DropGestureRecognizer { AllowDrop = true };
            dropGr.Drop += DropGr_Drop_Item;  // Drop Gesture for Items 
            dropGr.DragLeave += DropGr_DragLeave_Item; // Drag Leave Gesture for Items
            dropGr.DragOver += DropGr_DragOver_Item; // Drop Gesture for Items
            

            label.GestureRecognizers.Add(dropGr);
            label.GestureRecognizers.Add(dragGr);

            var xamlItemTreeViewNode = CreateTreeViewNode(xamlItem, label, true);
            children.Add(xamlItemTreeViewNode);
        }
        private void DropGr_DragOver_Item(object? sender, DragEventArgs e)
        {

#if WINDOWS
    var dragUI = e.PlatformArgs.DragEventArgs.DragUIOverride;
       dragUI.IsCaptionVisible = false;
        dragUI.IsGlyphVisible = false;
#endif
#if IOS || MACCATALYST
    e.PlatformArgs.SetDropProposal(new UIKit.UIDropProposal(UIKit.UIDropOperation.Move));
#endif
            if (!e.Data.Properties.ContainsKey("ItemId")) return;
            //var data = e.Data.Properties["Text"].ToString();
            var sourceItemId = e.Data.Properties["ItemId"].ToString();

            var label = (sender as Element)?.Parent as Label;
            var itemId = label.StyleId.Substring(5);

            if (sourceItemId != itemId) // Ignore if self
            {
                var _contentView = label.Parent as ContentView;
                _contentView.BackgroundColor = Microsoft.Maui.Graphics.Color.FromRgb(50, 205, 50); //Green 

            }
        }

        private void DropGr_DragLeave_Item(object? sender, DragEventArgs e)
        {
            if (!e.Data.Properties.ContainsKey("ItemId")) return;
            //var data = e.Data.Properties["Text"].ToString();
            var sourceItemId = e.Data.Properties["ItemId"].ToString();

            var label = (sender as Element)?.Parent as Label;
            var itemId = label.StyleId.Substring(5);
            if (sourceItemId != itemId) // Ignore if self
            {
                var _contentView = label.Parent as ContentView;
                _contentView.BackgroundColor = Microsoft.Maui.Graphics.Color.FromRgb(255, 255, 255); //White 

            }


        }


        private void DragGr_DragStarting_Item(object? sender, DragStartingEventArgs e)
        {
            var label = (sender as Element)?.Parent as Label;
            var itemId = label.StyleId.Substring(5);

            var _contentView = label.Parent as ContentView;
            //_contentView.BackgroundColor = Microsoft.Maui.Graphics.Color.FromRgb(50,205,50); //Green 

            //e.Data.Properties.Add("Text", label.Text);
            e.Data.Properties.Add("ItemId", itemId);

        }

        private void DropGr_Drop_Item(object? sender, DropEventArgs e)
        {
            //var data = e.Data.Properties["Text"].ToString();
            if (!e.Data.Properties.ContainsKey("ItemId")) return;
            var sourceItemId = e.Data.Properties["ItemId"].ToString();


            var label = (sender as Element)?.Parent as Label;
            var targetItemId = label.StyleId.Substring(5);

            var _contentView = label.Parent as ContentView;
            _contentView.BackgroundColor = Microsoft.Maui.Graphics.Color.FromRgb(255, 255, 255); //White 

            (this.BindingContext as ITreeViewPageViewModel).ReOrderItems(sourceItemId, targetItemId); 


          
        }

        // MAIN Method to generate XAML;  This method takes the data structures, and creates the Xaml using recursion
        public ObservableCollection<TreeViewNode> ProcessXamlItemGroups(XamlItemGroup xamlItemGroups)
        {
            var rootNodes = new ObservableCollection<TreeViewNode>();

            foreach (var xamlItemGroup in xamlItemGroups.Children)/*.OrderBy(xig => xig.Name)*/
            {
                var label = new Label
                {
                    VerticalOptions = LayoutOptions.Center,
                    TextColor = Colors.Black,
                    BackgroundColor = Colors.White

                };
                label.SetBinding(Label.TextProperty, "Name");
                // ================== This is where we want to put the Flyout Context Menu for the Folder ===========================================================

                RelativeBindingSource rbs = new RelativeBindingSource(RelativeBindingSourceMode.FindAncestorBindingContext, ancestorType: typeof(ITreeViewPageViewModel));

                var menuItem1 = new MenuFlyoutItem
                {
                    Text = "Add Folder",
                    CommandParameter = xamlItemGroup.GroupId,
                };
                menuItem1.Bind(MenuFlyoutItem.CommandProperty,
                    mode: BindingMode.OneTime,
                    source: rbs,
                    path: "AddFolderCommand"
                );


                var sep = new MenuFlyoutSeparator();
                var menuItem2 = new MenuFlyoutItem
                {
                    Text = "Add Item",
                    CommandParameter = xamlItemGroup.GroupId,
                };
                menuItem2.Bind(MenuFlyoutItem.CommandProperty,
                       mode: BindingMode.OneTime,
                       source: rbs,
                       path: "AddItemCommand"
                   );
                var menuItem3 = new MenuFlyoutItem
                {
                    Text = "Delete Folder",
                    CommandParameter = xamlItemGroup.GroupId,
                };
                menuItem3.Bind(MenuFlyoutItem.CommandProperty,
                       mode: BindingMode.OneTime,
                       source: rbs,
                       path: "DeleteFolderCommand"
                   );

                var menuItem4 = new MenuFlyoutItem
                {
                    Text = "Add Root Folder",
                    CommandParameter = xamlItemGroup.GroupId,
                };
                menuItem4.Bind(MenuFlyoutItem.CommandProperty,
                       mode: BindingMode.OneTime,
                       source: rbs,
                       path: "AddRootFolderCommand"
                   );


                var menuFlyout = new MenuFlyout();
                menuFlyout.Add(menuItem1);
                menuFlyout.Add(menuItem3);
                menuFlyout.Add(menuItem4);
                menuFlyout.Add(sep);
                menuFlyout.Add(menuItem2);

                FlyoutBase.SetContextFlyout(label, menuFlyout);


                // =============Drag and Drop for Nodes  =====================================================================================================================
                // This is required for the Drag/Drop events to be able to process the event arguments
                // by retrieving the NodeId 
                label.StyleId = "Node_" + xamlItemGroup.GroupId.ToString();

                // ================== DragStarting Gesture for Items =======================
                DragGestureRecognizer dragGr = new DragGestureRecognizer
                {
                    CanDrag = true
                };
                dragGr.DragStarting += DragGr_DragStarting_Node;

                // ================== Drop Gesture for Items ===============================
                DropGestureRecognizer dropGr = new DropGestureRecognizer { AllowDrop = true };
                dropGr.Drop += DropGr_Drop_Node;  // Drop Gesture for Nodes 
                dropGr.DragLeave += DropGr_DragLeave_Node; // Drag Leave Gesture for Nodes
                dropGr.DragOver += DropGr_DragOver_Node; // Drop Gesture for Nodes


                label.GestureRecognizers.Add(dropGr);
                label.GestureRecognizers.Add(dragGr);






                var groupTreeViewNode = CreateTreeViewNode(xamlItemGroup, label, false);

                rootNodes.Add(groupTreeViewNode);

                groupTreeViewNode.ChildrenList = ProcessXamlItemGroups(xamlItemGroup);

                foreach (var xamlItem in xamlItemGroup.XamlItems)
                {
                    CreateXamlItem(groupTreeViewNode.ChildrenList, xamlItem);
                }
            }

            return rootNodes;
        }
        private void DropGr_DragOver_Node(object? sender, DragEventArgs e)
        {

#if WINDOWS
    var dragUI = e.PlatformArgs.DragEventArgs.DragUIOverride;
       dragUI.IsCaptionVisible = false;
        dragUI.IsGlyphVisible = false;
#endif
#if IOS || MACCATALYST
    e.PlatformArgs.SetDropProposal(new UIKit.UIDropProposal(UIKit.UIDropOperation.Move));
#endif
            //var data = e.Data.Properties["Text"].ToString();
            if (!e.Data.Properties.ContainsKey("NodeId")) return;
            var sourceNodeId = e.Data.Properties["NodeId"].ToString();

            var label = (sender as Element)?.Parent as Label;
            var nodeId = label.StyleId.Substring(5);

            if (sourceNodeId != nodeId) // Ignore if self
            {
                var _contentView = label.Parent as ContentView;
                _contentView.BackgroundColor = Microsoft.Maui.Graphics.Color.FromRgb(50, 205, 50); //Green 

            }
        }

        private void DropGr_DragLeave_Node(object? sender, DragEventArgs e)
        {
            //var data = e.Data.Properties["Text"].ToString();
            if (!e.Data.Properties.ContainsKey("NodeId")) return;
            var sourceNodeId = e.Data.Properties["NodeId"].ToString();

            var label = (sender as Element)?.Parent as Label;
            var nodeId = label.StyleId.Substring(5);
            if (sourceNodeId != nodeId) // Ignore if self
            {
                var _contentView = label.Parent as ContentView;
                _contentView.BackgroundColor = Microsoft.Maui.Graphics.Color.FromRgb(255, 255, 255); //White 

            }


        }


        private void DragGr_DragStarting_Node(object? sender, DragStartingEventArgs e)
        {
            var label = (sender as Element)?.Parent as Label;
            var nodeId = label.StyleId.Substring(5);

            var _contentView = label.Parent as ContentView;
            //_contentView.BackgroundColor = Microsoft.Maui.Graphics.Color.FromRgb(50,205,50); //Green 

            //e.Data.Properties.Add("Text", label.Text);
            e.Data.Properties.Add("NodeId", nodeId);

        }

        private void DropGr_Drop_Node(object? sender, DropEventArgs e)
        {
            if (!e.Data.Properties.ContainsKey("NodeId")) return;
            //var data = e.Data.Properties["Text"].ToString();
            var sourceNodeId = e.Data.Properties["NodeId"].ToString();


            var label = (sender as Element)?.Parent as Label;
            var targetNodeId = label.StyleId.Substring(5);

            var _contentView = label.Parent as ContentView;
            _contentView.BackgroundColor = Microsoft.Maui.Graphics.Color.FromRgb(255, 255, 255); //White 

            (this.BindingContext as ITreeViewPageViewModel).ReOrderNodes(sourceNodeId, targetNodeId);



        }

    }
}
