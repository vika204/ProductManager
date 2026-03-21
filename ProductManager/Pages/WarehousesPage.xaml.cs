using ProductManager.ViewModels;

namespace ProductManager.Pages
{
    public partial class WarehousesPage : ContentPage
    {
        // page only receives view model from di and binds it
        public WarehousesPage(WarehousesViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }
    }
}