using ProductManager.ViewModels;

namespace ProductManager.Pages;

public partial class WarehouseUpsertPage : ContentPage
{
    private readonly WarehouseUpsertViewModel _viewModel;

    public WarehouseUpsertPage(WarehouseUpsertViewModel vm)
    {
        InitializeComponent();
        BindingContext = _viewModel = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.RefreshData();
    }
}