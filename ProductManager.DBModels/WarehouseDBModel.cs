using ProductManager.Common.Enums;
using System;

namespace ProductManager.DBModels
{
    public class WarehouseDBModel
    {
        // id property is readonly and is set through the constructor
        public Guid Id { get; }

        // name can be changed
        public string Name { get; set; }

        // location can be changed
        public WarehouseLocation Location { get; set; }

        private WarehouseDBModel() { }

        public WarehouseDBModel(string name, WarehouseLocation location)
        {
            Id = Guid.NewGuid();
            Name = name;
            Location = location;
        }
    }
}