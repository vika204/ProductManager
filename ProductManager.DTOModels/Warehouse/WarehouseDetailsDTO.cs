using ProductManager.Common.Enums;

namespace ProductManager.DTOModels.Warehouse
{
    // dto for warehouse details page
    // contains data for selected warehouse
    // it doesnt contain setters because it is only used to show data and should not be modified
    public class WarehouseDetailsDTO
    {
        public Guid Id { get; }

        public string Name { get; }

        public WarehouseLocation Location { get; }

        public decimal TotalValue { get; }

        public WarehouseDetailsDTO(Guid id, string name, WarehouseLocation location, decimal totalValue)
        {
            Id = id;
            Name = name;
            Location = location;
            TotalValue = totalValue;
        }
    }
}