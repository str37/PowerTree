using CommunityToolkit.Maui.Markup;
using PowerTree.Maui.Controls;
using PowerTree.Maui.Helpers;
using PowerTree.Maui.Interfaces;
using PowerTree.Maui.ViewModel;


namespace PowerTree.Maui.Controls;

public partial class PowerTree : ContentView
{
    private ITreeViewPageViewModel ViewModel;
    PowerTreeViewBuilder companyTreeViewBuilder;
    private TreeView TheTreeView;

    ITreeViewService _service;
    public PowerTree(ITreeViewPageViewModel viewModel, ITreeViewService service, PowerTreeViewBuilder companyTreeViewBuilder)
	{
        // InitializeComponent(); This is not needed since all UI elements are defined in c# code
        this.BindingContext = viewModel;
        this._service = service;
        this.companyTreeViewBuilder = companyTreeViewBuilder;
        ViewModel = viewModel;

        // Define the TreeView Control
        TheTreeView = new TreeView()
                            .Margin(4)
                            .BackgroundColor(Colors.White);


        Content = TheTreeView;

        // This preps the data to be passed to the TreeView
        var xamlItemGroups = companyTreeViewBuilder.GroupData(_service);

        // This creates all the rootnodes with nodes and items inside
        var rootNodes = TheTreeView.ProcessXamlItemGroups(xamlItemGroups);
        
        // Now set the RootNodes property of the control with the data so it may render
        TheTreeView.RootNodes = rootNodes;



    }


}