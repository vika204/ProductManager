using ProductManager.DBModels;

namespace ProductManager.Storage
{
 
    // storage context is responsible for providing raw data
    // it returns only db models and does not know about ui or dto
    public interface IStorageContext
    {
        // return all warehouses from storage
        IEnumerable<WarehouseDBModel> GetWarehouses();

        // return one warehouse by id
        WarehouseDBModel GetWarehouse(Guid warehouseId);

        // return products that belong to a warehouse
        IEnumerable<ProductDBModel> GetProducts(Guid warehouseId);

        // return one product by id
        ProductDBModel GetProduct(Guid productId);
    }
}