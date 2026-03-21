using ProductManager.Common.Enums;

namespace ProductManager.DTOModels.Warehouse
{
    // dto for warehouses list page
    // contains only data that is needed to show warehouse in a list
    // it doesnt contain setters because it is only used to show data and should not be modified
    public class WarehouseListDTO
    {
        public Guid Id { get; }

        public string Name { get; }

        public WarehouseLocation Location { get; }

        public int ProductsCount { get; }

        public WarehouseListDTO(Guid id, string name, WarehouseLocation location, int productsCount)
        {
            Id = id;
            Name = name;
            Location = location;
            ProductsCount = productsCount;
        }
    }
}