using ProductManager.DBModels;

namespace ProductManager.Repostory
{
    // warehouse repository provides access to warehouse db models
    // it does not contain business logic and does not return dto
    public interface IWarehouseRepository
    {
        // return all warehouses from storage
        IEnumerable<WarehouseDBModel> GetWarehouses();

        // return one warehouse by id
        WarehouseDBModel GetWarehouse(Guid warehouseId);
    }
}