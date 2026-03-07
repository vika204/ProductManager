using ProductManager.UIModels;

namespace ProductManager.Pages
{
    [QueryProperty(nameof(CurrentProduct), nameof(CurrentProduct))]
    public partial class ProductDetailsPage : ContentPage
    {
        // current product is passed through shell navigation
        private ProductUIModel? _currentProduct;

        // query property receives the selected product from the previous page
        public ProductUIModel CurrentProduct
        {
            get => _currentProduct ?? throw new InvalidOperationException("Current product was not set.");
            set
            {
                _currentProduct = value;
                // bind the page to the selected product
                BindingContext = CurrentProduct;
            }
        }

        public ProductDetailsPage()
        {
            InitializeComponent();
        }
    }
}