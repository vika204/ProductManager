using ProductManager.DBModels;

namespace ProductManager.Services
{
    public class StorageService
    {
        private List<WarehouseDBModel> _warehouses;
        private List<ProductDBModel> _products;

        private void LoadData()
        {
            if (_warehouses != null && _products != null)
            {
                return;
            }
            _warehouses = FakeStorage.Warehouses.ToList();
            _products = FakeStorage.Products.ToList();
        }

        public IEnumerable<WarehouseDBModel> GetWarehouses()
        {
            LoadData();
            return _warehouses.ToList();
        }

        public IEnumerable<ProductDBModel> GetProducts(Guid warehouseId)
        {
            LoadData();
            return _products.Where(p => p.WarehouseId == warehouseId).ToList();
        }
    }
}