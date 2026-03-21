using ProductManager.DTOModels.Warehouse;
using ProductManager.Repostory;

namespace ProductManager.Services
{
    // warehouse service contains business logic for warehouses
    // it gets db models from repositories and converts them into dto models
    public class WarehouseService : IWarehouseService
    {
        // repository for warehouse db models
        private readonly IWarehouseRepository _warehouseRepository;

        // repository for product db models
        // used here to calculate products count and total value for a warehouse
        private readonly IProductRepository _productRepository;

        public WarehouseService(IWarehouseRepository warehouseRepository, IProductRepository productRepository)
        {
            _warehouseRepository = warehouseRepository;
            _productRepository = productRepository;
        }

        // build list dto objects for the warehouses page
        public IEnumerable<WarehouseListDTO> GetWarehouses()
        {
            // list dto contains only data needed for list page
            return _warehouseRepository
                .GetWarehouses()
                .Select(warehouse => new WarehouseListDTO(
                    warehouse.Id,
                    warehouse.Name,
                    warehouse.Location,
                    _productRepository.GetProducts(warehouse.Id).Count()));
        }

        // build details dto object for the warehouse details page
        public WarehouseDetailsDTO GetWarehouse(Guid warehouseId)
        {
            // get warehouse db model
            var warehouse = _warehouseRepository.GetWarehouse(warehouseId);

            // calculate total value for the warehouse products
            var totalValue = _productRepository
                .GetProducts(warehouseId)
                .Sum(product => product.Price * product.Quantity);

            // return details dto for ui
            return new WarehouseDetailsDTO(warehouse.Id, warehouse.Name, warehouse.Location, totalValue);
        }
    }
}