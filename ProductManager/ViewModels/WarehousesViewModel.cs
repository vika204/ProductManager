using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProductManager.DTOModels.Warehouse;
using ProductManager.Pages;
using ProductManager.Services;
using System.Collections.ObjectModel;

namespace ProductManager.ViewModels
{
    public partial class WarehousesViewModel : BaseViewModel
    {
        private readonly IWarehouseService _warehouseService;

        // collection displayed on the main warehouses page.
        [ObservableProperty]
        private ObservableCollection<WarehouseListDTO> _warehouses;

        // selected warehouse in CollectionView.
        // it is used for navigation to details page.
        [ObservableProperty]
        private WarehouseListDTO? _currentWarehouse;

        // user-entered search text.
        [ObservableProperty]
        private string _searchText;

        // selected sort option from Picker.
        [ObservableProperty]
        private string _selectedSortOption;

        // sort options are taken from service constants
        // so that the same values are used in both layers.
        public string[] SortOptions =>
        [
            WarehouseService.SortByNameAscending,
            WarehouseService.SortByNameDescending,
            WarehouseService.SortByLocation,
            WarehouseService.SortByTotalValueDescending
        ];

        public WarehousesViewModel(IWarehouseService warehouseService)
        {
            _warehouseService = warehouseService;

            _warehouses = new ObservableCollection<WarehouseListDTO>();
            _searchText = string.Empty;
            _selectedSortOption = WarehouseService.SortByNameAscending;
        }

        [RelayCommand]
        internal async Task RefreshData()
        {
            // prevents duplicate concurrent refresh operations.
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                IEnumerable<WarehouseListDTO> warehouses = await _warehouseService.GetWarehousesAsync(SearchText, SelectedSortOption);
                Warehouses = new ObservableCollection<WarehouseListDTO>(warehouses);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync("Error", $"Failed to load warehouses: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task ApplyFilters()
        {
            //reuses the same loading method after search/sort values change.
            await RefreshData();
        }

        [RelayCommand]
        private async Task GotoWarehouse()
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
                Guid warehouseId = CurrentWarehouse.Id;

                // clear selection so that the same item can be tapped again later.
                CurrentWarehouse = null;

                await Shell.Current.GoToAsync(
                    nameof(WarehouseDetailsPage),
                    new Dictionary<string, object>
                    {
                        { "WarehouseId", warehouseId }
                    });
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync("Error", $"Failed to open warehouse details: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task AddWarehouse()
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // opens create form without parameters.
                await Shell.Current.GoToAsync(nameof(WarehouseUpsertPage));
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
        private async Task DeleteWarehouse(WarehouseListDTO warehouse)
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
                    $"Delete warehouse \"{warehouse.Name}\"?",
                    "Yes",
                    "No");

                if (!isConfirmed)
                {
                    return;
                }

                await _warehouseService.DeleteWarehouseAsync(warehouse.Id);

                // update UI after successful delete.
                Warehouses.Remove(warehouse);
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
    }
}