using ProductManager.Services;
using ProductManager.UIModels;

namespace ProductManager.Pages
{
    [QueryProperty(nameof(CurrentWarehouse), nameof(CurrentWarehouse))]
    public partial class WarehouseDetailsPage : ContentPage
    {
        // current warehouse is passed through shell navigation
        private WarehouseUIModel? _currentWarehouse;

        // query property receives the selected warehouse from the previous page
        public WarehouseUIModel CurrentWarehouse
        {
            get => _currentWarehouse ?? throw new InvalidOperationException("Current warehouse was not set.");
            set
            {
                _currentWarehouse = value;
                // load related products before binding to the page
                _currentWarehouse.LoadProducts();
                // bind the page to the selected warehouse
                BindingContext = CurrentWarehouse;
            }
        }

        public WarehouseDetailsPage(IStorageService storage)
        {
            InitializeComponent();
        }

        // open product details when user selects an item
        private async void ProductSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count == 0)
            {
                return;
            }

            ProductUIModel product = (ProductUIModel)e.CurrentSelection[0];

            await Shell.Current.GoToAsync(
                $"{nameof(ProductDetailsPage)}",
                new Dictionary<string, object>
                {
                    { nameof(ProductDetailsPage.CurrentProduct), product }
                });

            // clear selection so the same item can be tapped again later
            ((CollectionView)sender).SelectedItem = null;
        }
    }
}