using ProductManager.DBModels;
using ProductManager.Storage;

namespace ProductManager.Repostory
{
    // warehouse repository is a thin wrapper around storage context
    // it hides storage implementation from service layer
    public class WarehouseRepository : IWarehouseRepository
    {
        private readonly IStorageContext _storage;

        public WarehouseRepository(IStorageContext storage)
        {
            _storage = storage;
        }

        // return all warehouses from storage
        public Task<IEnumerable<WarehouseDBModel>> GetWarehousesAsync()
        {
            return _storage.GetWarehousesAsync();
        }

        // return one warehouse by id
        public Task<WarehouseDBModel?> GetWarehouseAsync(Guid warehouseId)
        {
            return _storage.GetWarehouseAsync(warehouseId);
        }

        // save warehouse
        public Task SaveWarehouseAsync(WarehouseDBModel warehouse)
        {
            return _storage.SaveWarehouseAsync(warehouse);
        }

        //delete watehouse by id
        public Task DeleteWarehouseAsync(Guid warehouseId)
        {
            return _storage.DeleteWarehouseAsync(warehouseId);
        }
    }
}