using ProductManager.Common.Enums;

namespace ProductManager.DTOModels.Product
{
    // dto for product details page
    // contains full product data needed for details screen
    // it doesnt contain setters because it is only used to show data and should not be modified
    public class ProductDetailsDTO
    {
        public Guid Id { get; }

        public Guid WarehouseId { get; }

        public string Name { get; }

        public int Quantity { get; }

        public decimal Price { get; }

        public ProductCategory Category { get; }

        public string Description { get; }

        public ProductDetailsDTO(Guid id, Guid warehouseId, string name, int quantity, decimal price, ProductCategory category, string description)
        {
            Id = id;
            WarehouseId = warehouseId;
            Name = name;
            Quantity = quantity;
            Price = price;
            Category = category;
            Description = description;
        }
    }
}