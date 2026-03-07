using ProductManager.DBModels;

namespace ProductManager.Services
{
    public interface IStorageService
    {
        // returns all warehouses from storage
        IEnumerable<WarehouseDBModel> GetWarehouses();

        // returns only products that belong to the selected warehouse
        IEnumerable<ProductDBModel> GetProducts(Guid warehouseId);
    }
}