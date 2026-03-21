using CommunityToolkit.Mvvm.ComponentModel;
using ProductManager.Common.Enums;
using ProductManager.DTOModels.Product;
using ProductManager.Services;
using System;
using System.Collections.Generic;

namespace ProductManager.ViewModels
{
    // view model for product details page
    // receives product id from shell navigation and loads product dto
    public partial class ProductDetailsViewModel : ObservableObject, IQueryAttributable
    {
        // current product is passed through shell navigation
        private readonly IProductService _productService;

        // loaded product details dto
        private ProductDetailsDTO? _currentProduct;

        // calculated value for ui
        private decimal _totalValue;

        // properties are used directly in xaml bindings
        public string Name => _currentProduct?.Name;

        public int Quantity => _currentProduct?.Quantity ?? 0;

        public decimal Price => _currentProduct?.Price ?? 0m;

        public ProductCategory? Category => _currentProduct?.Category;

        public string Description => _currentProduct?.Description;

        // calculated property for ui
        public decimal TotalValue => _totalValue;

        public ProductDetailsViewModel(IProductService productService)
        {
            _productService = productService;
        }

        // shell passes parameters into the view model
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            // get product id from previous page
            var productId = (Guid)query["ProductId"];

            // load product details dto
            _currentProduct = _productService.GetProduct(productId);

            // calculate total value for selected product
            CalculateTotalValue();

            // notify ui that dependent properties changed
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(Quantity));
            OnPropertyChanged(nameof(Price));
            OnPropertyChanged(nameof(Category));
            OnPropertyChanged(nameof(Description));
            OnPropertyChanged(nameof(TotalValue));
        }

        // calculate total value for the product
        private void CalculateTotalValue()
        {
            if (_currentProduct == null)
            {
                _totalValue = 0m;
                return;
            }

            _totalValue = _currentProduct.Price * _currentProduct.Quantity;
        }
    }
}