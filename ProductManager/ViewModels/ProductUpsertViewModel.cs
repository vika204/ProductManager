using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProductManager.Common;
using ProductManager.Common.Enums;
using ProductManager.DTOModels.Product;
using ProductManager.Services;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace ProductManager.ViewModels
{
    public partial class ProductUpsertViewModel : BaseViewModel, IQueryAttributable
    {
        private readonly IProductService _productService;

        // null means create mode, non-null means edit mode.
        private Guid? _productId;

        // product must always belong to some warehouse.
        private Guid _warehouseId;

        [ObservableProperty]
        private string _pageTitle;

        [ObservableProperty]
        private string _name;

        [ObservableProperty]
        private string _quantity;

        [ObservableProperty]
        private string _price;

        [ObservableProperty]
        private EnumWithName<ProductCategory>? _category;

        [ObservableProperty]
        private string _description;

        // validation messages for form fields.
        [ObservableProperty]
        private string _nameError;

        [ObservableProperty]
        private string _quantityError;

        [ObservableProperty]
        private string _priceError;

        [ObservableProperty]
        private string _categoryError;

        [ObservableProperty]
        private string _descriptionError;

        // Values for category Picker.
        public EnumWithName<ProductCategory>[] Categories { get; }

        public ProductUpsertViewModel(IProductService productService)
        {
            _productService = productService;

            _pageTitle = "Create Product";
            _name = string.Empty;
            _quantity = string.Empty;
            _price = string.Empty;
            _description = string.Empty;
            _nameError = string.Empty;
            _quantityError = string.Empty;
            _priceError = string.Empty;
            _categoryError = string.Empty;
            _descriptionError = string.Empty;

            Categories = EnumExtensions.GetValuesWithNames<ProductCategory>();
            Category = Categories.FirstOrDefault();
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            // warehouseId comes when user creates product from warehouse details page.
            if (query.TryGetValue("WarehouseId", out object? warehouseId))
            {
                _warehouseId = (Guid)warehouseId;
            }

            // productId comes when user edits an existing product.
            if (query.TryGetValue("ProductId", out object? productId))
            {
                _productId = (Guid)productId;
                PageTitle = "Edit Product";
            }
        }

        [RelayCommand]
        internal async Task RefreshData()
        {
            // in create mode there is nothing to preload.
            if (_productId is null || IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                ProductUpsertDTO product = await _productService.GetProductForEditAsync(_productId.Value)
                    ?? throw new InvalidOperationException("Product does not exist.");

                _warehouseId = product.WarehouseId;
                Name = product.Name;
                Quantity = product.Quantity.ToString(CultureInfo.InvariantCulture);
                Price = product.Price.ToString("0.##", CultureInfo.InvariantCulture);
                Category = Categories.FirstOrDefault(item => item.Value.Equals(product.Category));
                Description = product.Description;
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync("Error", $"Failed to load product form: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task SaveProduct()
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;
            ResetErrors();

            try
            {
                // parse quantity from string because Entry binds to string.
                int? parsedQuantity = int.TryParse(
                    Quantity,
                    NumberStyles.Integer,
                    CultureInfo.InvariantCulture,
                    out int quantityValue)
                    ? quantityValue
                    : null;

                // parse price in invariant culture.
                // replace ',' with '.' to avoid input problems.
                decimal? parsedPrice = decimal.TryParse(
                    Price?.Replace(',', '.'),
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out decimal priceValue)
                    ? priceValue
                    : null;

                List<ValidationResult> validationErrors = Validators.ValidateProduct(
                    _warehouseId,
                    Name,
                    parsedQuantity,
                    parsedPrice,
                    Category?.Value,
                    Description);

                if (validationErrors.Count > 0)
                {
                    FillErrors(validationErrors);
                    return;
                }

                ProductUpsertDTO product = new(
                    _warehouseId,
                    Name.Trim(),
                    parsedQuantity!.Value,
                    parsedPrice!.Value,
                    Category!.Value,
                    Description.Trim());

                if (_productId is null)
                {
                    await _productService.CreateProductAsync(product);
                }
                else
                {
                    await _productService.UpdateProductAsync(_productId.Value, product);
                }

                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync("Error", $"Failed to save product: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task Back()
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync("Error", $"Failed to navigate back: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void ResetErrors()
        {
            NameError = string.Empty;
            QuantityError = string.Empty;
            PriceError = string.Empty;
            CategoryError = string.Empty;
            DescriptionError = string.Empty;
        }

        private void FillErrors(IEnumerable<ValidationResult> validationErrors)
        {
            foreach (ValidationResult error in validationErrors)
            {
                foreach (string memberName in error.MemberNames)
                {
                    switch (memberName)
                    {
                        case nameof(Name):
                            NameError = AppendError(NameError, error.ErrorMessage);
                            break;

                        case nameof(Quantity):
                            QuantityError = AppendError(QuantityError, error.ErrorMessage);
                            break;

                        case nameof(Price):
                            PriceError = AppendError(PriceError, error.ErrorMessage);
                            break;

                        case nameof(Category):
                            CategoryError = AppendError(CategoryError, error.ErrorMessage);
                            break;

                        case nameof(Description):
                            DescriptionError = AppendError(DescriptionError, error.ErrorMessage);
                            break;
                    }
                }
            }
        }

        private static string AppendError(string currentValue, string? newError)
        {
            if (string.IsNullOrWhiteSpace(newError))
            {
                return currentValue;
            }

            if (string.IsNullOrWhiteSpace(currentValue))
            {
                return newError;
            }

            return currentValue + Environment.NewLine + newError;
        }
    }
}