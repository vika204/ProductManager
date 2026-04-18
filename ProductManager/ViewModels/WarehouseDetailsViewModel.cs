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
    public partial class WarehouseDetailsViewModel : BaseViewModel, IQueryAttributable
    {
        private readonly IWarehouseService _warehouseService;

        // product service provides products for selected warehouse
        private readonly IProductService _productService;

        // stores the identifier passed through Shell navigation.
        private Guid _warehouseId;

        // main warehouse data shown in details section.
        [ObservableProperty]
        private WarehouseDetailsDTO? _currentWarehouse;

        // list of products that belong to current warehouse.
        [ObservableProperty]
        private ObservableCollection<ProductListDTO> _products;

        // search text for products.
        [ObservableProperty]
        private string _searchText;

        // selected sort option for products list.
        [ObservableProperty]
        private string _selectedSortOption;

        public string[] SortOptions =>
        [
            ProductService.SortByNameAscending,
            ProductService.SortByNameDescending,
            ProductService.SortByQuantityDescending,
            ProductService.SortByPriceDescending,
            ProductService.SortByTotalValueDescending
        ];

        public WarehouseDetailsViewModel(IWarehouseService warehouseService, IProductService productService)
        {
            _warehouseService = warehouseService;
            _productService = productService;

            _products = new ObservableCollection<ProductListDTO>();
            _searchText = string.Empty;
            _selectedSortOption = ProductService.SortByNameAscending;
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            // receives WarehouseId from Shell navigation.
            if (query.TryGetValue("WarehouseId", out object? warehouseId))
            {
                _warehouseId = (Guid)warehouseId;
            }
        }

        [RelayCommand]
        internal async Task RefreshData()
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Load warehouse header data.
                CurrentWarehouse = await _warehouseService.GetWarehouseAsync(_warehouseId)
                    ?? throw new InvalidOperationException("Warehouse does not exist.");

                // Load related products list.
                IEnumerable<ProductListDTO> products = await _productService.GetProductsByWarehouseAsync(_warehouseId, SearchText, SelectedSortOption);
                Products = new ObservableCollection<ProductListDTO>(products);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync("Error", $"Failed to load warehouse details: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task ApplyFilters()
        {
            await RefreshData();
        }

        [RelayCommand]
        private async Task LoadProduct(Guid productId)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Opens product details page.
                await Shell.Current.GoToAsync(
                    nameof(ProductDetailsPage),
                    new Dictionary<string, object>
                    {
                        { "ProductId", productId }
                    });
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync("Error", $"Failed to open product details: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task AddProduct()
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Opens create form for product and passes parent warehouse id.
                await Shell.Current.GoToAsync(
                    nameof(ProductUpsertPage),
                    new Dictionary<string, object>
                    {
                        { "WarehouseId", _warehouseId }
                    });
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync("Error", $"Failed to open product form: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task EditWarehouse()
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Opens warehouse edit form and passes warehouse id.
                await Shell.Current.GoToAsync(
                    nameof(WarehouseUpsertPage),
                    new Dictionary<string, object>
                    {
                        { "WarehouseId", _warehouseId }
                    });
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync("Error", $"Failed to open warehouse form: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task DeleteWarehouse()
        {
            if (IsBusy)
            {
                return;
            }

            if (CurrentWarehouse is null)
            {
                return;
            }

            IsBusy = true;

            try
            {
                bool isConfirmed = await Shell.Current.DisplayAlertAsync(
                    "Confirm",
                    $"Delete warehouse \"{CurrentWarehouse.Name}\" and all its products?",
                    "Yes",
                    "No");

                if (!isConfirmed)
                {
                    return;
                }

                await _warehouseService.DeleteWarehouseAsync(_warehouseId);

                // Return to warehouses list after successful delete.
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync("Error", $"Failed to delete warehouse: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task DeleteProduct(ProductListDTO product)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                bool isConfirmed = await Shell.Current.DisplayAlertAsync(
                    "Confirm",
                    $"Delete product \"{product.Name}\"?",
                    "Yes",
                    "No");

                if (!isConfirmed)
                {
                    return;
                }

                await _productService.DeleteProductAsync(product.Id);

                // Remove deleted item from current UI collection.
                Products.Remove(product);

                // Refresh warehouse summary because total value/products count may change.
                CurrentWarehouse = await _warehouseService.GetWarehouseAsync(_warehouseId)
                    ?? throw new InvalidOperationException("Warehouse does not exist.");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync("Error", $"Failed to delete product: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}