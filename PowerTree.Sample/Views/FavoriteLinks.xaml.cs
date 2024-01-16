
using PowerTree.Maui;
using PowerTree.Sample.ViewModel;

namespace PowerTree.Sample.Views;

public partial class FavoriteLinksPage : ContentPage
{
    private string _subSystem = "Favorites"; // This defines the PowerTree Control Hierarchy Name
    public FavoriteLinksPage(FavoriteLinksViewModel viewModel) 
    {
        InitializeComponent();

        mainGrid.Children.Add(PowerTreeInitializer.CreatePowerTreeControl(viewModel, _subSystem));

    }
}