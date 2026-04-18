using ProductManager.Common.Enums;

namespace ProductManager.DTOModels.Product
{
    // dto for products list inside warehouse details page
    // contains info for list view
    // it doesnt contain setters because it is only used to show data and should not be modified
    public class ProductListDTO
    {
        public Guid Id { get; }

        public string Name { get; }

        public ProductCategory Category { get; }

        public int Quantity { get; }

        public decimal Price { get; }

        public decimal TotalValue { get; }

        public ProductListDTO(Guid id, string name, ProductCategory category, int quantity, decimal price, decimal totalValue)
        {
            Id = id;
            Name = name;
            Category = category;
            Quantity = quantity;
            Price = price;
            TotalValue = totalValue;
        }
    }
}