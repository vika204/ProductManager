using ProductManager.DBModels;
using ProductManager.Storage;

namespace ProductManager.Repostory
{
    // warehouse repository is a thin wrapper around storage context
    // it hides storage implementation from service layer
    public class WarehouseRepository : IWarehouseRepository
    {
        // storage context provides raw db models
        private readonly IStorageContext _storage;

        public WarehouseRepository(IStorageContext storage)
        {
            _storage = storage;
        }

        // return all warehouses from storage
        public IEnumerable<WarehouseDBModel> GetWarehouses()
        {
            return _storage.GetWarehouses();
        }

        // return one warehouse by id
        public WarehouseDBModel GetWarehouse(Guid warehouseId)
        {
            return _storage.GetWarehouse(warehouseId);
        }
    }
}