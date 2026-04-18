using ProductManager.DTOModels.Warehouse;

namespace ProductManager.Services
{
    // warehouse service provides warehouse data for ui
    // it returns dto models and does not expose db models
    public interface IWarehouseService
    {
        // returns filtered and sorted list of warehouses for main page.
        Task<IEnumerable<WarehouseListDTO>> GetWarehousesAsync(string? searchText, string? sortOption);

        // returns detailed data for a specific warehouse.
        Task<WarehouseDetailsDTO?> GetWarehouseAsync(Guid warehouseId);

        // returns warehouse data prepared for edit form.
        Task<WarehouseUpsertDTO?> GetWarehouseForEditAsync(Guid warehouseId);

        // creates a new warehouse.
        Task CreateWarehouseAsync(WarehouseUpsertDTO warehouse);

        // updates an existing warehouse.
        Task UpdateWarehouseAsync(Guid warehouseId, WarehouseUpsertDTO warehouse);

        // Deletes warehouse by identifier.
        Task DeleteWarehouseAsync(Guid warehouseId);
    }
}