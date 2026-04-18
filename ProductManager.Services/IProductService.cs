using ProductManager.DTOModels.Product;

namespace ProductManager.Services
{
    // product service provides product data for ui
    // it returns dto models and does not expose db models
    public interface IProductService
    {
        //returns filtered and sorted list of products for a specific warehouse
        Task<IEnumerable<ProductListDTO>> GetProductsByWarehouseAsync(Guid warehouseId, string? searchText, string? sortOption);

        // returns detailed data for one product
        Task<ProductDetailsDTO?> GetProductAsync(Guid productId);

        // returns product data prepared for edit form
        Task<ProductUpsertDTO?> GetProductForEditAsync(Guid productId);

        // creates a new product
        Task CreateProductAsync(ProductUpsertDTO product);

        // updates an existing product.
        Task UpdateProductAsync(Guid productId, ProductUpsertDTO product);

        // deletes product by id.
        Task DeleteProductAsync(Guid productId);
    }
}