using ProductManager.DBModels;

namespace ProductManager.Repostory
{
    // warehouse repository provides access to warehouse db models
    // it does not contain business logic and does not return dto
    public interface IWarehouseRepository
    {
        // returns all warehouses from storage.
        Task<IEnumerable<WarehouseDBModel>> GetWarehousesAsync();

        // returns one warehouse by identifier or null if it does not exist.
        Task<WarehouseDBModel?> GetWarehouseAsync(Guid warehouseId);

        // creates a new warehouse or updates an existing one.
        Task SaveWarehouseAsync(WarehouseDBModel warehouse);

        // deletes warehouse by identifier.
        Task DeleteWarehouseAsync(Guid warehouseId);
    }
}