using ProductManager.Common.Enums;
using System;

namespace ProductManager.DBModels
{

    public class ProductDBModel
    {
        // the id is unchangeable
        public Guid Id { get; }

        // the product can be moved to another warehouse
        public Guid WarehouseId { get; set; }

        // name, quantity, price, category and description can be changed
        public string Name { get; set; }
        public int Quantity { get; set; }

        // set decimal type because it is more precise for financial calculations
        public decimal Price { get; set; }
        public ProductCategory Category { get; set; }
        public string Description { get; set; }

        private ProductDBModel() { }


        // we initialize all the properties through the constructor. id is generated automatically.
        public ProductDBModel(Guid warehouseId, string name, int quantity, decimal price, ProductCategory category, string description)
        {
            Id = Guid.NewGuid();
            WarehouseId = warehouseId;
            Name = name;
            Quantity = quantity;
            Price = price;
            Category = category;
            Description = description;
        }
    }
}