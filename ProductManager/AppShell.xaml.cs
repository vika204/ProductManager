using ProductManager.Pages;

namespace ProductManager
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(WarehouseDetailsPage), typeof(WarehouseDetailsPage));
            Routing.RegisterRoute(nameof(ProductDetailsPage), typeof(ProductDetailsPage));
            Routing.RegisterRoute(nameof(WarehouseUpsertPage), typeof(WarehouseUpsertPage));
            Routing.RegisterRoute(nameof(ProductUpsertPage), typeof(ProductUpsertPage));
        }
    }
}