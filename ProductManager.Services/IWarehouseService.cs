using ProductManager.DTOModels.Warehouse;

namespace ProductManager.Services
{
    // warehouse service provides warehouse data for ui
    // it returns dto models and does not expose db models
    public interface IWarehouseService
    {
        // list page data
        IEnumerable<WarehouseListDTO> GetWarehouses();

        // list page data
        WarehouseDetailsDTO GetWarehouse(Guid warehouseId);
    }
}