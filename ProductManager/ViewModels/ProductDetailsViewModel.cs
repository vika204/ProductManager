using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProductManager.DTOModels.Product;
using ProductManager.Pages;
using ProductManager.Services;

namespace ProductManager.ViewModels
{
    public partial class ProductDetailsViewModel : BaseViewModel, IQueryAttributable
    {
        private readonly IProductService _productService;

        // product id received from navigation.
        private Guid _productId;

        // data shown on product details page.
        [ObservableProperty]
        private ProductDetailsDTO? _currentProduct;

        public ProductDetailsViewModel(IProductService productService)
        {
            _productService = productService;
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            // receives ProductId from Shell navigation.
            if (query.TryGetValue("ProductId", out object? productId))
            {
                _productId = (Guid)productId;
            }
        }

        [RelayCommand]
        internal async Task RefreshData()
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                CurrentProduct = await _productService.GetProductAsync(_productId)
                    ?? throw new InvalidOperationException("Product does not exist.");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Failed to load product details: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task EditProduct()
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // opens product edit form with existing product id.
                await Shell.Current.GoToAsync(
                    nameof(ProductUpsertPage),
                    new Dictionary<string, object>
                    {
                        { "ProductId", _productId }
                    });
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Failed to open product form: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task DeleteProduct()
        {
            if (IsBusy)
            {
                return;
            }

            if (CurrentProduct is null)
            {
                return;
            }

            IsBusy = true;

            try
            {
                bool isConfirmed = await Shell.Current.DisplayAlert(
                    "Confirm",
                    $"Delete product \"{CurrentProduct.Name}\"?",
                    "Yes",
                    "No");

                if (!isConfirmed)
                {
                    return;
                }

                await _productService.DeleteProductAsync(_productId);

                // return back after successful delete.
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Failed to delete product: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}