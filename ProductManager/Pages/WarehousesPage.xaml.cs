using System.Collections.ObjectModel;
using ProductManager.Services;
using ProductManager.UIModels;

namespace ProductManager.Pages
{
    public partial class WarehousesPage : ContentPage
    {
        // storage service provides warehouses for the start page
        private readonly IStorageService _storage;

        // observable collection is used so the ui can react to changes
        public ObservableCollection<WarehouseUIModel> Warehouses { get; set; }

        public WarehousesPage(IStorageService storageService)
        {
            InitializeComponent();

            _storage = storageService;
            Warehouses = new ObservableCollection<WarehouseUIModel>();

            // convert db models into ui models for display
            foreach (var warehouse in _storage.GetWarehouses())
            {
                Warehouses.Add(new WarehouseUIModel(_storage, warehouse));
            }

            // bind the page to its own public properties
            BindingContext = this;
        }

        // open warehouse details when user selects an item
        private async void WarehouseSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count == 0)
            {
                return;
            }

            WarehouseUIModel warehouse = (WarehouseUIModel)e.CurrentSelection[0];

            await Shell.Current.GoToAsync(
                $"{nameof(WarehouseDetailsPage)}",
                new Dictionary<string, object>
                {
                    { nameof(WarehouseDetailsPage.CurrentWarehouse), warehouse }
                });

            // clear selection so the same item can be tapped again later
            ((CollectionView)sender).SelectedItem = null;
        }
    }
}