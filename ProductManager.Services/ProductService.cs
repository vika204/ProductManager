using ProductManager.DBModels;
using ProductManager.DTOModels.Product;
using ProductManager.Repostory;
using System.ComponentModel.DataAnnotations;

namespace ProductManager.Services
{
    // product service contains business logic for products
    // it converts db models from repository into dto models for ui
    public class ProductService : IProductService
    {
        // Sort option constants are shared with the UI layer.
        public const string SortByNameAscending = "Name (A-Z)";
        public const string SortByNameDescending = "Name (Z-A)";
        public const string SortByQuantityDescending = "Quantity";
        public const string SortByPriceDescending = "Price";
        public const string SortByTotalValueDescending = "Total value";

        // repository for product db models
        private readonly IProductRepository _productRepository;
        // repository for warehouse db models
        private readonly IWarehouseRepository _warehouseRepository;
        public ProductService(IProductRepository productRepository, IWarehouseRepository warehouseRepository)
        {
            _productRepository = productRepository;
            _warehouseRepository = warehouseRepository;
        }

        public async Task<IEnumerable<ProductListDTO>> GetProductsByWarehouseAsync(Guid warehouseId, string? searchText, string? sortOption)
        {
            IEnumerable<ProductDBModel> products = await _productRepository.GetProductsByWarehouseAsync(warehouseId);

            // product list DTO additionally contains computed TotalValue field.
            List<ProductListDTO> productDtos = products
                .Select(product => new ProductListDTO(
                    product.Id,
                    product.Name,
                    product.Category,
                    product.Quantity,
                    product.Price,
                    product.Price * product.Quantity))
                .ToList();

            // search works by product name and category text.
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                productDtos = productDtos
                    .Where(product =>
                        product.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                        product.Category.ToString().Contains(searchText, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // sorting is performed according to user-selected option.
            productDtos = sortOption switch
            {
                SortByNameDescending => productDtos.OrderByDescending(product => product.Name).ToList(),
                SortByQuantityDescending => productDtos.OrderByDescending(product => product.Quantity).ThenBy(product => product.Name).ToList(),
                SortByPriceDescending => productDtos.OrderByDescending(product => product.Price).ThenBy(product => product.Name).ToList(),
                SortByTotalValueDescending => productDtos.OrderByDescending(product => product.TotalValue).ThenBy(product => product.Name).ToList(),
                _ => productDtos.OrderBy(product => product.Name).ToList()
            };

            return productDtos;
        }

        public async Task<ProductDetailsDTO?> GetProductAsync(Guid productId)
        {
            ProductDBModel? product = await _productRepository.GetProductAsync(productId);

            if (product is null)
            {
                return null;
            }

            return new ProductDetailsDTO(
                product.Id,
                product.WarehouseId,
                product.Name,
                product.Quantity,
                product.Price,
                product.Category,
                product.Description);
        }

        public async Task<ProductUpsertDTO?> GetProductForEditAsync(Guid productId)
        {
            ProductDBModel? product = await _productRepository.GetProductAsync(productId);

            if (product is null)
            {
                return null;
            }

            // upsert DTO contains only fields needed in form.
            return new ProductUpsertDTO(
                product.WarehouseId,
                product.Name,
                product.Quantity,
                product.Price,
                product.Category,
                product.Description);
        }

        public async Task CreateProductAsync(ProductUpsertDTO product)
        {
            // product must always be attached to an existing warehouse.
            WarehouseDBModel existingWarehouse = await _warehouseRepository.GetWarehouseAsync(product.WarehouseId)
                ?? throw new InvalidOperationException("Warehouse does not exist.");

            List<ValidationResult> errors = Validators.ValidateProduct(
                existingWarehouse.Id,
                product.Name,
                product.Quantity,
                product.Price,
                product.Category,
                product.Description);

            if (errors.Count > 0)
            {
                throw new ValidationException(errors.First().ErrorMessage);
            }

            ProductDBModel productDbModel = new(
                product.WarehouseId,
                product.Name.Trim(),
                product.Quantity,
                product.Price,
                product.Category,
                product.Description.Trim());

            await _productRepository.SaveProductAsync(productDbModel);
        }

        public async Task UpdateProductAsync(Guid productId, ProductUpsertDTO product)
        {
            ProductDBModel existingProduct = await _productRepository.GetProductAsync(productId)
                ?? throw new InvalidOperationException("Product does not exist.");

            WarehouseDBModel existingWarehouse = await _warehouseRepository.GetWarehouseAsync(product.WarehouseId)
                ?? throw new InvalidOperationException("Warehouse does not exist.");

            List<ValidationResult> errors = Validators.ValidateProduct(
                existingWarehouse.Id,
                product.Name,
                product.Quantity,
                product.Price,
                product.Category,
                product.Description);

            if (errors.Count > 0)
            {
                throw new ValidationException(errors.First().ErrorMessage);
            }

            // we update editable fields of existing product.
            existingProduct.WarehouseId = product.WarehouseId;
            existingProduct.Name = product.Name.Trim();
            existingProduct.Quantity = product.Quantity;
            existingProduct.Price = product.Price;
            existingProduct.Category = product.Category;
            existingProduct.Description = product.Description.Trim();

            await _productRepository.SaveProductAsync(existingProduct);
        }

        public Task DeleteProductAsync(Guid productId)
        {
            return _productRepository.DeleteProductAsync(productId);
        }

    }
}