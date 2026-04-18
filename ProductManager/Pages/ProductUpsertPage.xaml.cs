using ProductManager.ViewModels;

namespace ProductManager.Pages;

public partial class ProductUpsertPage : ContentPage
{
    private readonly ProductUpsertViewModel _viewModel;

    public ProductUpsertPage(ProductUpsertViewModel vm)
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