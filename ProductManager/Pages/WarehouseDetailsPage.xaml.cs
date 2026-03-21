using ProductManager.ViewModels;

namespace ProductManager.Pages
{
    public partial class WarehouseDetailsPage : ContentPage
    {
        // view model receives warehouse id via iqueryattributable
        // page only binds view model from di
        public WarehouseDetailsPage(WarehouseDetailsViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }
    }
}