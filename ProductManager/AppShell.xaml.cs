using ProductManager.Pages;

namespace ProductManager
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // register nested routes for shell navigation
            Routing.RegisterRoute(
                $"{nameof(WarehousesPage)}/{nameof(WarehouseDetailsPage)}",
                typeof(WarehouseDetailsPage));

            Routing.RegisterRoute(
                $"{nameof(WarehousesPage)}/{nameof(WarehouseDetailsPage)}/{nameof(ProductDetailsPage)}",
                typeof(ProductDetailsPage));
        }
    }
}