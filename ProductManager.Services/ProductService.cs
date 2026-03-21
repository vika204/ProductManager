using ProductManager.DTOModels.Product;
using ProductManager.Repostory;

namespace ProductManager.Services
{
    // product service contains business logic for products
    // it converts db models from repository into dto models for ui
    public class ProductService : IProductService
    {        
        // repository for product db models
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // build list dto objects for the warehouse details page
        public IEnumerable<ProductListDTO> GetProductsByWarehouse(Guid warehouseId)
        {
            // list dto contains only info for list ui
            return _productRepository
                .GetProducts(warehouseId)
                .Select(product => new ProductListDTO(product.Id, product.Name, product.Category));
        }

        // build details dto object for the product details page
        public ProductDetailsDTO GetProduct(Guid productId)
        {
            // get product db model
            var product = _productRepository.GetProduct(productId);

            // return details dto for ui
            return new ProductDetailsDTO(
                product.Id,
                product.WarehouseId,
                product.Name,
                product.Quantity,
                product.Price,
                product.Category,
                product.Description);
        }
    }
}