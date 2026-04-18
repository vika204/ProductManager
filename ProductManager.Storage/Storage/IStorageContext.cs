using ProductManager.DBModels;

namespace ProductManager.Storage
{
    public interface IStorageContext
    {
        // Warehouse operations
        Task<IEnumerable<WarehouseDBModel>> GetWarehousesAsync();
        Task<WarehouseDBModel?> GetWarehouseAsync(Guid warehouseId);
        Task SaveWarehouseAsync(WarehouseDBModel warehouse);
        Task DeleteWarehouseAsync(Guid warehouseId);

        // Product operations
        Task<IEnumerable<ProductDBModel>> GetProductsByWarehouseAsync(Guid warehouseId);
        Task<ProductDBModel?> GetProductAsync(Guid productId);
        Task SaveProductAsync(ProductDBModel product);
        Task DeleteProductAsync(Guid productId);
    }
}