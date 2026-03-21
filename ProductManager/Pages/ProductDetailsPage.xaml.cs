using ProductManager.ViewModels;

namespace ProductManager.Pages
{
    public partial class ProductDetailsPage : ContentPage
    {
        // view model receives product id via iqueryattributable
        // page only binds view model from di
        public ProductDetailsPage(ProductDetailsViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }
    }
}