using ProductManager.DBModels;
using ProductManager.DTOModels.Warehouse;
using ProductManager.Repostory;
using System.ComponentModel.DataAnnotations;

namespace ProductManager.Services
{
    // warehouse service contains business logic for warehouses
    // it gets db models from repositories and converts them into dto models
    public class WarehouseService : IWarehouseService
    {

        // sort option constants are reused in viewmodels.
        public const string SortByNameAscending = "Name (A-Z)";
        public const string SortByNameDescending = "Name (Z-A)";
        public const string SortByLocation = "Location";
        public const string SortByTotalValueDescending = "Total value";

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

        public async Task<IEnumerable<WarehouseListDTO>> GetWarehousesAsync(string? searchText, string? sortOption)
        {
            IEnumerable<WarehouseDBModel> warehouses = await _warehouseRepository.GetWarehousesAsync();
            List<WarehouseListDTO> warehouseDtos = new();

            // For each warehouse we calculate number of products and total value of all products in that warehouse
            foreach (WarehouseDBModel warehouse in warehouses)
            {
                IEnumerable<ProductDBModel> products = await _productRepository.GetProductsByWarehouseAsync(warehouse.Id);

                warehouseDtos.Add(new WarehouseListDTO(
                    warehouse.Id,
                    warehouse.Name,
                    warehouse.Location,
                    products.Sum(product => product.Price * product.Quantity)));
            }

            // search is applied after DTO creation because UI works with DTO data.
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                warehouseDtos = warehouseDtos
                    .Where(warehouse =>
                        warehouse.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                        warehouse.Location.ToString().Contains(searchText, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // sorting is done according to the selected option.
            warehouseDtos = sortOption switch
            {
                SortByNameDescending => warehouseDtos.OrderByDescending(warehouse => warehouse.Name).ToList(),
                SortByLocation => warehouseDtos.OrderBy(warehouse => warehouse.Location).ThenBy(warehouse => warehouse.Name).ToList(),
                SortByTotalValueDescending => warehouseDtos.OrderByDescending(warehouse => warehouse.TotalValue).ThenBy(warehouse => warehouse.Name).ToList(),
                _ => warehouseDtos.OrderBy(warehouse => warehouse.Name).ToList()
            };

            return warehouseDtos;
        }

        public async Task<WarehouseDetailsDTO?> GetWarehouseAsync(Guid warehouseId)
        {
            WarehouseDBModel? warehouse = await _warehouseRepository.GetWarehouseAsync(warehouseId);

            if (warehouse is null)
            {
                return null;
            }

            IEnumerable<ProductDBModel> products = await _productRepository.GetProductsByWarehouseAsync(warehouseId);

            return new WarehouseDetailsDTO(
                warehouse.Id,
                warehouse.Name,
                warehouse.Location,
                products.Sum(product => product.Price * product.Quantity));
        }

        public async Task<WarehouseUpsertDTO?> GetWarehouseForEditAsync(Guid warehouseId)
        {
            WarehouseDBModel? warehouse = await _warehouseRepository.GetWarehouseAsync(warehouseId);

            if (warehouse is null)
            {
                return null;
            }

            // upsert DTO contains only fields needed in edit form.
            return new WarehouseUpsertDTO(warehouse.Name, warehouse.Location);
        }

        public async Task CreateWarehouseAsync(WarehouseUpsertDTO warehouse)
        {
            List<ValidationResult> errors = Validators.ValidateWarehouse(warehouse.Name, warehouse.Location);

            if (errors.Count > 0)
            {
                throw new ValidationException(errors.First().ErrorMessage);
            }

            WarehouseDBModel warehouseDbModel = new(warehouse.Name.Trim(), warehouse.Location);
            await _warehouseRepository.SaveWarehouseAsync(warehouseDbModel);
        }

        public async Task UpdateWarehouseAsync(Guid warehouseId, WarehouseUpsertDTO warehouse)
        {
            WarehouseDBModel existingWarehouse = await _warehouseRepository.GetWarehouseAsync(warehouseId)
                ?? throw new InvalidOperationException("Warehouse does not exist.");

            List<ValidationResult> errors = Validators.ValidateWarehouse(warehouse.Name, warehouse.Location);

            if (errors.Count > 0)
            {
                throw new ValidationException(errors.First().ErrorMessage);
            }

            // we update only editable fields.
            existingWarehouse.Name = warehouse.Name.Trim();
            existingWarehouse.Location = warehouse.Location;

            await _warehouseRepository.SaveWarehouseAsync(existingWarehouse);
        }

        public Task DeleteWarehouseAsync(Guid warehouseId)
        {
            // cascade deletion of products is handled by the storage layer
            return _warehouseRepository.DeleteWarehouseAsync(warehouseId);
        }

    }
}