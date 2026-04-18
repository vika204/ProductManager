using ProductManager.Common.Enums;

namespace ProductManager.DTOModels.Product
{
    public class ProductUpsertDTO
    {
        // warehouseId is required because product cannot exist without warehouse.
        public Guid WarehouseId { get; }

        // editable fields of product entity.
        public string Name { get; }
        public int Quantity { get; }
        public decimal Price { get; }
        public ProductCategory Category { get; }
        public string Description { get; }

        public ProductUpsertDTO(Guid warehouseId, string name, int quantity, decimal price, ProductCategory category, string description)
        {
            WarehouseId = warehouseId;
            Name = name;
            Quantity = quantity;
            Price = price;
            Category = category;
            Description = description;
        }
    }
}