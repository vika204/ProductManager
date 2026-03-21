using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProductManager.DTOModels.Product;
using ProductManager.DTOModels.Warehouse;
using ProductManager.Pages;
using ProductManager.Services;
using System.Collections.ObjectModel;

namespace ProductManager.ViewModels
{
    // view model for warehouse details page
    // receives warehouse id from shell navigation and loads dto models
    public partial class WarehouseDetailsViewModel : ObservableObject, IQueryAttributable
    {
        // current warehouse is passed through shell navigation
        private readonly IWarehouseService _warehouseService;

        // product service provides products for selected warehouse
        private readonly IProductService _productService;

        // selected warehouse details dto
        [ObservableProperty]
        private WarehouseDetailsDTO? _currentWarehouse;

        // products list dto for selected warehouse
        [ObservableProperty]
        private ObservableCollection<ProductListDTO> _products;

        public WarehouseDetailsViewModel(IWarehouseService warehouseService, IProductService productService)
        {
            _warehouseService = warehouseService;
            _productService = productService;
        }

        // shell passes parameters into the view model
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (!query.ContainsKey("WarehouseId"))
            {
                return;
            }

            // get warehouse id from previous page
            var warehouseId = (Guid)query["WarehouseId"];

            // load warehouse details dto
            CurrentWarehouse = _warehouseService.GetWarehouse(warehouseId);

            // load products list dto
            Products = new ObservableCollection<ProductListDTO>(_productService.GetProductsByWarehouse(warehouseId));
        }

        // open product details when user taps a product card
        [RelayCommand]
        private void LoadProduct(Guid productId)
        {
            Shell.Current.GoToAsync(
                $"{nameof(ProductDetailsPage)}",
                new Dictionary<string, object> { { "ProductId", productId } });
        }
    }
}