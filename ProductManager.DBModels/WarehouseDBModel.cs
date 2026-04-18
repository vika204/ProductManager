using ProductManager.Common.Enums;
using SQLite;

namespace ProductManager.DBModels
{
    [Table("Warehouses")]
    public class WarehouseDBModel
    {
        // Primary key of warehouse entity in the database.
        [PrimaryKey]
        public Guid Id { get; set; }

        // name can be changed
        [NotNull]
        public string Name { get; set; }

        // location can be changed
        public WarehouseLocation Location { get; set; }

        // parameterless constructor is required by SQLite.
        public WarehouseDBModel()
        {
            Name = string.Empty;
        }

        // сonstructor for creating a new warehouse
        public WarehouseDBModel(string name, WarehouseLocation location)
            : this(Guid.NewGuid(), name, location)
        {
        }

        // сonstructor for creating an object with a predefined identifier.
        public WarehouseDBModel(Guid id, string name, WarehouseLocation location)
        {
            Id = id;
            Name = name;
            Location = location;
        }
    }
}