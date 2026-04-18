using ProductManager.Common.Enums;

namespace ProductManager.DTOModels.Warehouse
{
    public class WarehouseUpsertDTO
    {
        // only fields needed for create/edit are included here.
        public string Name { get; }
        public WarehouseLocation Location { get; }

        public WarehouseUpsertDTO(string name, WarehouseLocation location)
        {
            Name = name;
            Location = location;
        }
    }
}