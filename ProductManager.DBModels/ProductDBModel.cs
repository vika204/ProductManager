using ProductManager.Common.Enums;
using SQLite;

namespace ProductManager.DBModels
{

    [Table("Products")]
    public class ProductDBModel
    {

        //primary key of product entity in the database.
        [PrimaryKey]
        public Guid Id { get; set; }

        // foreign key to warehouse.
        //one product belongs to exactly one warehouse.
        [Indexed]
        public Guid WarehouseId { get; set; }

        // name, quantity, price, category and description can be changed
        [NotNull]
        public string Name { get; set; }
        public int Quantity { get; set; }

        // set decimal type because it is more precise for financial calculations
        public decimal Price { get; set; }
        public ProductCategory Category { get; set; }
       
        [NotNull]
        public string Description { get; set; }

        // parameterless constructor is required by SQLite.
        public ProductDBModel()
        {
            Name = string.Empty;
            Description = string.Empty;
        }

        // constructor for creating a new product
        public ProductDBModel(Guid warehouseId, string name, int quantity, decimal price, ProductCategory category, string description)
            : this(Guid.NewGuid(), warehouseId, name, quantity, price, category, description)
        {
        }

        //constructor with explicit identifier
        public ProductDBModel(Guid id, Guid warehouseId, string name, int quantity, decimal price, ProductCategory category, string description)
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