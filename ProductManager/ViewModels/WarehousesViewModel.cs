using ProductManager.DTOModels.Warehouse;
using ProductManager.Pages;
using ProductManager.Services;
using System.Collections.ObjectModel;

namespace ProductManager.ViewModels
{
    // view model for warehouses list page
    // keeps list data and handles navigation to details
    public class WarehousesViewModel
    {
        // warehouse service provides dto for list page
        private readonly IWarehouseService _warehouseService;

        // observable collection is used so the ui can react to changes
        public ObservableCollection<WarehouseListDTO> Warehouses { get; set; }

        // selected warehouse from the list
        public WarehouseListDTO CurrentWarehouse { get; set; }

        // open warehouse details when user selects an item
        public Command WarehouseSelectedCommand { get; }

        public WarehousesViewModel(IWarehouseService warehouseService)
        {
            _warehouseService = warehouseService;

            // load warehouses for the start page
            Warehouses = new ObservableCollection<WarehouseListDTO>(_warehouseService.GetWarehouses());

            // command is called on selection change
            WarehouseSelectedCommand = new Command(LoadWarehouse);
        }

        // navigate to warehouse details page with selected warehouse id
        private void LoadWarehouse()
        {
            if (CurrentWarehouse != null)
            {
                Shell.Current.GoToAsync(
                    $"{nameof(WarehouseDetailsPage)}",
                    new Dictionary<string, object> { { "WarehouseId", CurrentWarehouse.Id } });
            }
        }
    }
}