using ProductManager.ViewModels;

namespace ProductManager.Pages;

public partial class WarehousesPage : ContentPage
{
    private readonly WarehousesViewModel _viewModel;

    public WarehousesPage(WarehousesViewModel vm)
    {
        InitializeComponent();
        BindingContext = _viewModel = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // refreshes data every time the page becomes visible.
        await _viewModel.RefreshData();
    }
}