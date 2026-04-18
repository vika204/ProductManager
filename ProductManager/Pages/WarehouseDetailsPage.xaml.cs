using ProductManager.ViewModels;

namespace ProductManager.Pages;

public partial class WarehouseDetailsPage : ContentPage
{
    private readonly WarehouseDetailsViewModel _viewModel;

    public WarehouseDetailsPage(WarehouseDetailsViewModel vm)
    {
        InitializeComponent();
        BindingContext = _viewModel = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // reloads warehouse details and related products each time page is shown.
        await _viewModel.RefreshData();
    }
}