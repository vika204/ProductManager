using ProductManager.DBModels;

namespace ProductManager.Repostory
{
    // product repository provides access to product db models
    // it does not contain business logic and does not return dto
    public interface IProductRepository
    {
        // returns all products that belong to a specific warehouse.
        Task<IEnumerable<ProductDBModel>> GetProductsByWarehouseAsync(Guid warehouseId);

        // returns one product by identifier or null if it does not exist.
        Task<ProductDBModel?> GetProductAsync(Guid productId);

        // creates a new product or updates an existing one.
        Task SaveProductAsync(ProductDBModel product);

        // deletes product by identifier.
        Task DeleteProductAsync(Guid productId);
    }
}